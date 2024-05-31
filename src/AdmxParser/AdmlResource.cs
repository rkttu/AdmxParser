using AdmxParser.Contracts;
using AdmxParser.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AdmxParser
{
    /// <summary>
    /// Represents an ADML resource.
    /// </summary>
    public class AdmlResource : ILocalizable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdmlResource"/> class.
        /// </summary>
        /// <param name="admlFilePath">The path to the ADML file.</param>
        public AdmlResource(string admlFilePath)
        {
            Helpers.EnsureFileExists(admlFilePath);

            _createdAdmlDataList = new HashSet<AdmlData>();

            _admlFilePath = admlFilePath;
            _loaded = false;
            
            _stringTable = new Dictionary<string, string>();
            _stringKeys = new List<string>();

            _readOnlyStringTable = new ReadOnlyDictionary<string, string>(_stringTable);
            _readOnlyStringKeys = new ReadOnlyCollection<string>(_stringKeys);
        }

        private string _pathPrefix;
        private XmlNamespaceManager _nsManager;
        private readonly HashSet<AdmlData> _createdAdmlDataList;

        private readonly string _admlFilePath;
        private bool _loaded;
        private string _displayName;
        private string _description;

        private readonly Dictionary<string, string> _stringTable;
        private readonly List<string> _stringKeys;

        private readonly IReadOnlyDictionary<string, string> _readOnlyStringTable;
        private readonly IReadOnlyList<string> _readOnlyStringKeys;

        internal string PathPrefix => _pathPrefix;
        internal XmlNamespaceManager NamespaceManager => _nsManager;

        /// <summary>
        /// Gets the path to the ADML file.
        /// </summary>
        public string AdmlFilePath => _admlFilePath;

        /// <summary>
        /// Gets a value indicating whether the ADML content is loaded.
        /// </summary>
        public bool Loaded => _loaded;

        /// <summary>
        /// Gets the display name of the ADML resource.
        /// </summary>
        public string DisplayName => _displayName;

        /// <summary>
        /// Gets the description of the ADML resource.
        /// </summary>
        public string Description => _description;

        /// <summary>
        /// Gets the string table of the ADML resource.
        /// </summary>
        public IReadOnlyDictionary<string, string> StringTable => _readOnlyStringTable;

        /// <summary>
        /// Gets the string keys of the ADML resource.
        /// </summary>
        public IReadOnlyList<string> StringKeys => _readOnlyStringKeys;

        /// <summary>
        /// Creates an ADML data instance.
        /// </summary>
        /// <typeparam name="TAdmlData">
        /// The type of the ADML data.
        /// </typeparam>
        /// <param name="sourceElement">
        /// The source <see cref="XElement"/>.
        /// </param>
        /// <returns>
        /// The created ADML data instance.
        /// </returns>
        public TAdmlData CreateAdmlData<TAdmlData>(XElement sourceElement)
            where TAdmlData : AdmlData
        {
            var data = (TAdmlData)typeof(TAdmlData).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.CreateInstance | BindingFlags.Instance,
                default, new Type[] { this.GetType(), typeof(XElement), }, default
            ).Invoke(new object[] { this, sourceElement, });

            _createdAdmlDataList.Add(data);
            return data;
        }

        /// <summary>
        /// Loads the ADML content asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task LoadAdmlAsync(CancellationToken cancellationToken = default)
        {
            if (_loaded)
                throw new InvalidOperationException("Already ADML content loaded.");

            Helpers.EnsureFileExists(_admlFilePath);

            using (var stream = File.OpenRead(_admlFilePath))
            {
                var streamReader = new StreamReader(stream, true);
                var admlContent = await streamReader.ReadToEndAsync().ConfigureAwait(false);
                var admlDocument = XDocument.Parse(admlContent, LoadOptions.None);

                _nsManager = Helpers.DiscoverDefaultNamespacePrefix(admlDocument, "policyDefinitionResources", out _pathPrefix, out _, nameTable: default);
                var rootElement = default(XElement);

                rootElement = admlDocument.XPathSelectElement($"/{_pathPrefix}policyDefinitionResources", _nsManager);
                if (rootElement == default)
                    _pathPrefix = string.Empty;

                rootElement = admlDocument.XPathSelectElement($"/{_pathPrefix}policyDefinitionResources", _nsManager);
                if (rootElement == default)
                    throw new XmlException("Root element is not a policyDefinitionResources element.");

                _displayName = rootElement.XPathSelectElement($"./{_pathPrefix}displayName", _nsManager)?.Value ?? string.Empty;
                _description = rootElement.XPathSelectElement($"./{_pathPrefix}description", _nsManager)?.Value ?? string.Empty;

                var stringTableElements = rootElement.XPathSelectElements($"./{_pathPrefix}resources/{_pathPrefix}stringTable/{_pathPrefix}string", _nsManager);
                foreach (var eachStringElement in stringTableElements)
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;

                    var eachId = eachStringElement.Attribute("id")?.Value;

                    if (eachId == default)
                        continue;

                    var eachValue = eachStringElement.Value;
                    _stringTable.Add(eachId, eachValue);
                    _stringKeys.Add(eachId);
                }

                _loaded = true;
            }
        }

        /// <summary>
        /// Gets the string value associated with the specified resource key.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <returns>The string value associated with the resource key.</returns>
        public string GetString(string resourceKey)
            => _stringTable[resourceKey];

        /// <summary>
        /// Tries to get the string value associated with the specified resource key.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="resourceString">When this method returns, contains the string value associated with the resource key, if the key is found; otherwise, an empty string.</param>
        /// <returns><c>true</c> if the resource key is found; otherwise, <c>false</c>.</returns>
        public bool TryGetString(string resourceKey, out string resourceString)
            => _stringTable.TryGetValue(resourceKey, out resourceString);
    }

}
