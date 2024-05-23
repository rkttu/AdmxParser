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
    public class AdmlResource : ILocalizable
    {
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

        public string AdmlFilePath => _admlFilePath;
        public bool Loaded => _loaded;
        public string DisplayName => _displayName;
        public string Description => _description;
        public IReadOnlyDictionary<string, string> StringTable => _stringTable;
        public IReadOnlyList<string> StringKeys => _stringKeys;

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

        public string GetString(string resourceKey)
            => _stringTable[resourceKey];

        public bool TryGetString(string resourceKey, out string resourceString)
            => _stringTable.TryGetValue(resourceKey, out resourceString);
    }

}
