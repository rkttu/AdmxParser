using AdmxParser.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace AdmxParser
{
    /// <summary>
    /// Represents an ADML resource.
    /// </summary>
    public class AdmlResource
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
        }

        private readonly string _admlFilePath;
        private bool _loaded;
        private PolicyDefinitionResources _policyDefinitionResources;
        private IReadOnlyDictionary<string, string> _stringTable;
        private IReadOnlyList<string> _stringKeys;

        /// <summary>
        /// Gets the path to the ADML file.
        /// </summary>
        public string AdmlFilePath => _admlFilePath;

        /// <summary>
        /// Gets a value indicating whether the ADML content is loaded.
        /// </summary>
        public bool Loaded => _loaded;

        /// <summary>
        /// Gets the policy definition resources of this ADML resource.
        /// </summary>
        public PolicyDefinitionResources PolicyDefinitionResources => _policyDefinitionResources;

        /// <summary>
        /// Gets the display name of the ADML resource.
        /// </summary>
        public string DisplayName => _policyDefinitionResources.displayName;

        /// <summary>
        /// Gets the description of the ADML resource.
        /// </summary>
        public string Description => _policyDefinitionResources.description;

        /// <summary>
        /// Gets the string table of the ADML resource.
        /// </summary>
        public IReadOnlyDictionary<string, string> StringTable => _stringTable;

        /// <summary>
        /// Gets the presentation table of the ADML resource.
        /// </summary>
        public IReadOnlyList<PolicyPresentation> PresentationTable => _policyDefinitionResources.resources.presentationTable.presentation;

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

            using (var fileStream = File.OpenRead(_admlFilePath))
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
                    var serializer = new XmlSerializer(typeof(PolicyDefinitionResources));
                    _policyDefinitionResources = (PolicyDefinitionResources)serializer.Deserialize(docReader);
                    _stringTable = new ReadOnlyDictionary<string, string>(_policyDefinitionResources.resources.stringTable.@string.ToDictionary(x => x.id, x => x.Value));
                    _stringKeys = _stringTable.Keys.ToArray();
                    _loaded = true;
                }
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
