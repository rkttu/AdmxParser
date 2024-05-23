using AdmxParser.Contracts;
using System.Xml.Linq;

namespace AdmxParser.Models
{
    public class MinorVersion : AdmxData, IHasNameAttribute, ILocalizable
    {
        protected MinorVersion(AdmxContent parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            _name = sourceElement.Attribute("name")?.Value;
            _displayName = sourceElement.Attribute("displayName")?.Value;
            _versionIndex = int.TryParse(sourceElement.Attribute("versionIndex")?.Value, out int versionIndex) ? versionIndex : default;
        }

        private readonly string _name;
        private readonly string _displayName;
        private readonly int? _versionIndex;

        public string Name => _name;
        public string DisplayName => _displayName;
        public int? VersionIndex => _versionIndex;
    }

}
