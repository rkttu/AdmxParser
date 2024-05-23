using AdmxParser.Contracts;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AdmxParser.Models
{
    public class MajorVersion : AdmxData, IHasNameAttribute, ILocalizable
    {
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

        public string Name => _name;
        public string DisplayName => _displayName;
        public int? VersionIndex => _versionIndex;
        public IReadOnlyList<MinorVersion> MinorVersions => _minorVersionsReadOnly;
    }

}
