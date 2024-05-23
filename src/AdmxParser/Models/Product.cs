using AdmxParser.Contracts;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AdmxParser.Models
{
    public class Product : AdmxData, IHasNameAttribute, ILocalizable
    {
        protected Product(AdmxContent parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            _name = sourceElement.Attribute("name")?.Value;
            _displayName = sourceElement.Attribute("displayName")?.Value;

            _majorVersions = new List<MajorVersion>();
            _majorVersionsReadOnly = new ReadOnlyCollection<MajorVersion>(_majorVersions);

            var pathPrefix = Parent.PathPrefix;
            var nsManager = Parent.NamespaceManager;

            var majorVersionElems = sourceElement.XPathSelectElements($"./{pathPrefix}majorVersion", nsManager);
            foreach (var eachMajorVersionElem in majorVersionElems)
                _majorVersions.Add(Parent.CreateAdmxData<MajorVersion>(eachMajorVersionElem));
        }

        private readonly string _name;
        private readonly string _displayName;

        private readonly List<MajorVersion> _majorVersions;
        private readonly ReadOnlyCollection<MajorVersion> _majorVersionsReadOnly;

        public string Name => _name;
        public string DisplayName => _displayName;
        public IReadOnlyList<MajorVersion> MajorVersions => _majorVersionsReadOnly;
    }

}
