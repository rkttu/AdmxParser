using AdmxParser.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
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
            _admlFilePath = admlFilePath;
            _loaded = false;
            _stringTable = new Dictionary<string, string>();
            _stringKeys = new List<string>();
        }

        private readonly string _admlFilePath;
        private bool _loaded;
        private string _displayName;
        private string _description;
        private readonly Dictionary<string, string> _stringTable;
        private readonly List<string> _stringKeys;

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
        public IReadOnlyDictionary<string, string> StringTable => _stringTable;

        /// <summary>
        /// Gets the string keys of the ADML resource.
        /// </summary>
        public IReadOnlyList<string> StringKeys => _stringKeys;

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

                var nsManager = Helpers.DiscoverDefaultNamespacePrefix(admlDocument, "policyDefinitionResources", out string pathPrefix, out _, nameTable: default);
                var rootElement = default(XElement);

                rootElement = admlDocument.XPathSelectElement($"/{pathPrefix}policyDefinitionResources", nsManager);
                if (rootElement == default)
                    pathPrefix = string.Empty;

                rootElement = admlDocument.XPathSelectElement($"/{pathPrefix}policyDefinitionResources", nsManager);
                if (rootElement == default)
                    throw new XmlException("Root element is not a policyDefinitionResources element.");

                _displayName = rootElement.XPathSelectElement($"./{pathPrefix}displayName", nsManager)?.Value ?? string.Empty;
                _description = rootElement.XPathSelectElement($"./{pathPrefix}description", nsManager)?.Value ?? string.Empty;

                var stringTableElements = rootElement.XPathSelectElements($"./{pathPrefix}resources/{pathPrefix}stringTable/{pathPrefix}string", nsManager);
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
