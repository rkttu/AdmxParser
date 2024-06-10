using AdmxParser.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

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

            _admxFilePath = admxFilePath;
            _loaded = false;
            _loadedAdmlResources = new Dictionary<CultureInfo, AdmlResource>();
            _loadedAdmlResourcesReadOnly = new ReadOnlyDictionary<CultureInfo, AdmlResource>(_loadedAdmlResources);
        }

        private readonly string _admxFilePath;
        private bool _loaded;
        private PolicyDefinitions _policyDefinitions;
        private readonly Dictionary<CultureInfo, AdmlResource> _loadedAdmlResources;
        private readonly IReadOnlyDictionary<CultureInfo, AdmlResource> _loadedAdmlResourcesReadOnly;

        /// <summary>
        /// Gets the path to the ADMX file.
        /// </summary>
        public string AdmxFilePath => _admxFilePath;

        /// <summary>
        /// Gets a value indicating whether the ADMX content is loaded.
        /// </summary>
        public bool Loaded => _loaded;

        /// <summary>
        /// Gets the policy definitions of this ADMX content.
        /// </summary>
        public PolicyDefinitions PolicyDefinitions => _policyDefinitions;

        /// <summary>
        /// Gets the target namespace of this ADMX content.
        /// </summary>
        public PolicyNamespaceAssociation TargetNamespace => _policyDefinitions.policyNamespaces.target;

        /// <summary>
        /// Gets the superseded ADM of this ADMX content.
        /// </summary>
        public FileReference[] SupersededAdm => _policyDefinitions.supersededAdm;

        /// <summary>
        /// Gets the resources of this ADMX content.
        /// </summary>
        public LocalizationResourceReference Resources => _policyDefinitions.resources;

        /// <summary>
        /// Gets the supported products and definitions of this ADMX content.
        /// </summary>
        public SupportedOnTable SupportedOn => _policyDefinitions.supportedOn;

        /// <summary>
        /// Gets the loaded ADML resources.
        /// </summary>
        public IReadOnlyDictionary<CultureInfo, AdmlResource> LoadedAdmlResources => _loadedAdmlResourcesReadOnly;

        /// <summary>
        /// Gets the using namespaces of this ADMX content.
        /// </summary>
        public IReadOnlyList<PolicyNamespaceAssociation> UsingNamespaces => _policyDefinitions.policyNamespaces.@using;

        /// <summary>
        /// Gets the categories of this ADMX content.
        /// </summary>
        public IReadOnlyList<Category> Categories => _policyDefinitions.categories.category;

        /// <summary>
        /// Gets the policies of this ADMX content.
        /// </summary>
        public IReadOnlyList<PolicyDefinition> Policies => _policyDefinitions.policies.policy;

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
        /// Load the ADMX content asynchronously.
        /// </summary>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the ADMX content is already loaded.
        /// </exception>
        /// <exception cref="XmlException">
        /// Thrown when the XML content is invalid.
        /// </exception>
        protected async Task LoadAdmxAsync(CancellationToken cancellationToken = default)
        {
            if (_loaded)
                throw new InvalidOperationException("Already ADMX content loaded.");

            Helpers.EnsureFileExists(_admxFilePath);

            using (var fileStream = File.OpenRead(_admxFilePath))
            using (var streamReader = new StreamReader(fileStream, new UTF8Encoding(false), true))
            {
                var content = await streamReader.ReadToEndAsync().ConfigureAwait(false);
                var stringReader = new StringReader(content);
                var xmlDoc = XDocument.Parse(content);
                var root = xmlDoc.Root;

                var xmlns = XNamespace.Get("http://www.microsoft.com/GroupPolicy/PolicyDefinitions");
                foreach (var eachElement in xmlDoc.Root.DescendantsAndSelf())
                    eachElement.Name = xmlns + eachElement.Name.LocalName;

                using (var docReader = xmlDoc.CreateReader())
                {
                    var serializer = new XmlSerializer(typeof(PolicyDefinitions));
                    _policyDefinitions = (PolicyDefinitions)serializer.Deserialize(docReader);
                    _loaded = true;
                }
            }
        }

        /// <summary>
        /// Load the ADML content asynchronously.
        /// </summary>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// </returns>
        /// <exception cref="IOException">
        /// Thrown when the directory path of the ADMX file cannot be obtained.
        /// </exception>
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
                _loadedAdmlResources.Add(foundCulture, eachAdmlResource);
            }
        }

        /// <summary>
        /// Gets the string value associated with the specified resource key.
        /// </summary>
        /// <param name="resourceKey">
        /// The resource key.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <param name="allowFallbackToEnglishUnitedStates">
        /// Whether to allow fallback to English (United States) if the specified culture is not found.
        /// </param>
        /// <returns>
        /// The string value associated with the resource key. If the resource key is not found, returns null.
        /// </returns>
        public string GetString(string resourceKey, CultureInfo culture, bool allowFallbackToEnglishUnitedStates = true)
        {
            if (_loadedAdmlResources.TryGetValue(culture, out var admlResource))
                return admlResource.GetString(resourceKey);

            if (allowFallbackToEnglishUnitedStates)
            {
                var enUsCulture = _loadedAdmlResources.Keys.FirstOrDefault(x => x.Name.Equals("en-US", StringComparison.OrdinalIgnoreCase));

                if (enUsCulture != null && _loadedAdmlResources.TryGetValue(enUsCulture, out var enUsAdmlResource))
                    return enUsAdmlResource.GetString(resourceKey);
            }

            return null;
        }
    }
}
