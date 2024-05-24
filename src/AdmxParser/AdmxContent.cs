using AdmxParser.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AdmxParser
{
    /// <summary>
    /// Represents an ADMX contents.
    /// </summary>
    public class AdmxContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdmxContent"/> class.
        /// </summary>
        /// <param name="admxFilePath">The path to the ADMX file.</param>
        public AdmxContent(string admxFilePath)
        {
            Helpers.EnsureFileExists(admxFilePath);

            _createdAdmxDataList = new HashSet<AdmxData>();

            _admxFilePath = admxFilePath;
            _loaded = false;
            _pathPrefix = default;
            _targetNamespace = default;
            _resources = default;
            _supportedOn = default;

            _loadedAdmlResources = new Dictionary<string, AdmlResource>();
            _usingNamespaces = new List<UsingNamespace>();
            _categories = new List<Category>();
            _policies = new List<Policy>();

            _loadedAdmlResourcesReadOnly = new ReadOnlyDictionary<string, AdmlResource>(_loadedAdmlResources);
            _usingNamespacesReadOnly = new ReadOnlyCollection<UsingNamespace>(_usingNamespaces);
            _categoriesReadOnly = new ReadOnlyCollection<Category>(_categories);
            _policiesReadOnly = new ReadOnlyCollection<Policy>(_policies);
        }

        private string _pathPrefix;
        private XmlNamespaceManager _nsManager;
        private readonly HashSet<AdmxData> _createdAdmxDataList;

        private readonly string _admxFilePath;
        private bool _loaded;
        private TargetNamespace _targetNamespace;
        private SupersededAdm _supersededAdm;
        private Resources _resources;
        private SupportedOn _supportedOn;

        private readonly Dictionary<string, AdmlResource> _loadedAdmlResources;
        private readonly List<UsingNamespace> _usingNamespaces;
        private readonly List<Category> _categories;
        private readonly List<Policy> _policies;

        private readonly IReadOnlyDictionary<string, AdmlResource> _loadedAdmlResourcesReadOnly;
        private readonly IReadOnlyList<UsingNamespace> _usingNamespacesReadOnly;
        private readonly IReadOnlyList<Category> _categoriesReadOnly;
        private readonly IReadOnlyList<Policy> _policiesReadOnly;

        internal string PathPrefix => _pathPrefix;
        internal XmlNamespaceManager NamespaceManager => _nsManager;
        internal HashSet<AdmxData> CreatedAdmxDataList => _createdAdmxDataList;

        /// <summary>
        /// Gets the path to the ADMX file.
        /// </summary>
        public string AdmxFilePath => _admxFilePath;

        /// <summary>
        /// Gets a value indicating whether the ADMX content is loaded.
        /// </summary>
        public bool Loaded => _loaded;

        /// <summary>
        /// Gets the target namespace of this ADMX content.
        /// </summary>
        public TargetNamespace TargetNamespace => _targetNamespace;

        /// <summary>
        /// Gets the superseded ADM of this ADMX content.
        /// </summary>
        public SupersededAdm SupersededAdm => _supersededAdm;
        
        /// <summary>
        /// Gets the resources of this ADMX content.
        /// </summary>
        public Resources Resources => _resources;
        
        /// <summary>
        /// Gets the supported products and definitions of this ADMX content.
        /// </summary>
        public SupportedOn SupportedOn => _supportedOn;

        /// <summary>
        /// Gets the loaded ADML resources.
        /// </summary>
        public IReadOnlyDictionary<string, AdmlResource> LoadedAdmlResources => _loadedAdmlResourcesReadOnly;

        /// <summary>
        /// Gets the using namespaces of this ADMX content.
        /// </summary>
        public IReadOnlyList<UsingNamespace> UsingNamespaces => _usingNamespacesReadOnly;

        /// <summary>
        /// Gets the categories of this ADMX content.
        /// </summary>
        public IReadOnlyList<Category> Categories => _categoriesReadOnly;

        /// <summary>
        /// Gets the policies of this ADMX content.
        /// </summary>
        public IReadOnlyList<Policy> Policies => _policiesReadOnly;

        /// <summary>
        /// Parse the XML element and create an ADMX data.
        /// </summary>
        /// <typeparam name="TAdmxData">The type of AdmxData you want to create</typeparam>
        /// <param name="sourceElement">Source XML element</param>
        /// <returns>Returns a new instance in the specified 'TAdmxData' format.</returns>
        public TAdmxData CreateAdmxData<TAdmxData>(XElement sourceElement)
            where TAdmxData : AdmxData
        {
            var data = (TAdmxData)typeof(TAdmxData).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.CreateInstance | BindingFlags.Instance,
                default, new Type[] { this.GetType(), typeof(XElement), }, default
            ).Invoke(new object[] { this, sourceElement, });

            _createdAdmxDataList.Add(data);
            return data;
        }

        /// <summary>
        /// Load the ADMX content asynchronously.
        /// </summary>
        /// <param name="loadAdml">Whether to also attempt to load the ADML file contained under the directory where the ADMX file is stored.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task LoadAsync(bool loadAdml = true, CancellationToken cancellationToken = default)
        {
            await LoadAdmxAsync(cancellationToken).ConfigureAwait(false);

            if (loadAdml)
                await LoadAdmlAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="XmlException"></exception>
        protected async Task LoadAdmxAsync(CancellationToken cancellationToken = default)
        {
            if (_loaded)
                throw new InvalidOperationException("Already ADMX content loaded.");

            Helpers.EnsureFileExists(_admxFilePath);

            using (var stream = File.OpenRead(_admxFilePath))
            {
                var streamReader = new StreamReader(stream, true);
                var admxContent = await streamReader.ReadToEndAsync().ConfigureAwait(false);
                var admxDocument = XDocument.Parse(admxContent, LoadOptions.None);

                _nsManager = Helpers.DiscoverDefaultNamespacePrefix(admxDocument, "policyDefinitions", out _pathPrefix, out _, nameTable: default);
                var rootElement = default(XElement);

                rootElement = admxDocument.XPathSelectElement($"/{_pathPrefix}policyDefinitions", _nsManager);
                if (rootElement == default)
                    throw new XmlException("Root element is not a policyDefinitions element.");

                var policyNamespacesElement = rootElement.XPathSelectElement($"./{_pathPrefix}policyNamespaces", _nsManager);
                if (policyNamespacesElement == default)
                    throw new XmlException("The policyNamespaces element is missing.");

                var targetElement = policyNamespacesElement.XPathSelectElement($"./{_pathPrefix}target", _nsManager);
                if (targetElement == default)
                    throw new XmlException("The target element is missing.");

                _targetNamespace = this.CreateAdmxData<TargetNamespace>(targetElement);

                var usingElements = policyNamespacesElement.XPathSelectElements($"./{_pathPrefix}using", _nsManager);
                foreach (var eachUsingElement in usingElements)
                    _usingNamespaces.Add(this.CreateAdmxData<UsingNamespace>(eachUsingElement));

                var supersededElement = rootElement.XPathSelectElement($"./{_pathPrefix}supersededAdm", _nsManager);
                if (supersededElement != default)
                    _supersededAdm = this.CreateAdmxData<SupersededAdm>(supersededElement);

                var resourcesElement = rootElement.XPathSelectElement($"./{_pathPrefix}resources", _nsManager);
                if (resourcesElement != default)
                    _resources = this.CreateAdmxData<Resources>(resourcesElement);

                var categoriesElement = rootElement.XPathSelectElement($"./{_pathPrefix}categories", _nsManager);
                if (categoriesElement != default)
                {
                    var categoryElements = categoriesElement.XPathSelectElements($"./{_pathPrefix}category", _nsManager);
                    foreach (var eachCategoryElement in categoryElements)
                        _categories.Add(this.CreateAdmxData<Category>(eachCategoryElement));
                }

                var supportedOnElem = rootElement.XPathSelectElement($"./{_pathPrefix}supportedOn", _nsManager);
                if (supportedOnElem != default)
                    _supportedOn = this.CreateAdmxData<SupportedOn>(supportedOnElem);

                var policiesElem = rootElement.XPathSelectElement($"./{_pathPrefix}policies", _nsManager);
                if (policiesElem != default)
                {
                    var policyElements = policiesElem.XPathSelectElements($"./{_pathPrefix}policy", _nsManager);
                    foreach (var eachPolicyElem in policyElements)
                        _policies.Add(this.CreateAdmxData<Policy>(eachPolicyElem));
                }

                _loaded = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="IOException"></exception>
        protected async Task LoadAdmlAsync(CancellationToken cancellationToken = default)
        {
            var directoryPath = Path.GetDirectoryName(_admxFilePath);

            if (directoryPath == default)
                throw new IOException($"Cannot obtain the directory path of '{_admxFilePath}'");

            var admxFileNameWithoutExt = Path.GetFileNameWithoutExtension(_admxFilePath);
            var win32Cultures = Helpers.AvailableCultures;

            foreach (var eachSubDirectory in Helpers.SafeEnumerateDirectories(directoryPath, "*.*", SearchOption.TopDirectoryOnly))
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                var itemName = Path.GetFileName(eachSubDirectory);
                var foundCulture = win32Cultures.FirstOrDefault(x => x.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));

                if (foundCulture == default)
                    continue;

                var inferredAdmlFilePath = Path.Combine(directoryPath, itemName, $"{admxFileNameWithoutExt}.adml");

                if (!File.Exists(inferredAdmlFilePath))
                    continue;

                var eachAdmlResource = new AdmlResource(inferredAdmlFilePath);
                await eachAdmlResource.LoadAdmlAsync(cancellationToken).ConfigureAwait(false);
                _loadedAdmlResources.Add(itemName, eachAdmlResource);
            }
        }
    }

}
