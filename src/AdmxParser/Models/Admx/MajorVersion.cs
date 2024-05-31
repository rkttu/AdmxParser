using AdmxParser.Contracts;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AdmxParser.Models.Admx
{
    /// <summary>
    /// Represents a major version in the ADMX data.
    /// </summary>
    public class MajorVersion : AdmxData, IHasNameAttribute, ILocalizable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MajorVersion"/> class.
        /// </summary>
        /// <param name="parent">The parent ADMX content.</param>
        /// <param name="sourceElement">The source XML element.</param>
        protected MajorVersion(AdmxContent parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            _name = sourceElement.Attribute("name")?.Value;
            _displayName = sourceElement.Attribute("displayName")?.Value;
            _versionIndex = int.TryParse(sourceElement.Attribute("versionIndex")?.Value, out int versionIndex) ? versionIndex : default;

            _minorVersions = new List<MinorVersion>();
            _minorVersionsReadOnly = new ReadOnlyCollection<MinorVersion>(_minorVersions);

            var pathPrefix = Parent.PathPrefix;
            var nsManager = Parent.NamespaceManager;

            var minorVersionElems = sourceElement.XPathSelectElements($"./{pathPrefix}minorVersion", nsManager);
            foreach (var eachMinorVersionElem in minorVersionElems)
                _minorVersions.Add(Parent.CreateAdmxData<MinorVersion>(eachMinorVersionElem));
        }

        private readonly string _name;
        private readonly string _displayName;
        private readonly int? _versionIndex;

        private readonly List<MinorVersion> _minorVersions;
        private readonly ReadOnlyCollection<MinorVersion> _minorVersionsReadOnly;

        /// <summary>
        /// Gets the name of the major version.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Gets the display name of the major version.
        /// </summary>
        public string DisplayName => _displayName;

        /// <summary>
        /// Gets the version index of the major version.
        /// </summary>
        public int? VersionIndex => _versionIndex;

        /// <summary>
        /// Gets the read-only list of minor versions.
        /// </summary>
        public IReadOnlyList<MinorVersion> MinorVersions => _minorVersionsReadOnly;
    }

}
