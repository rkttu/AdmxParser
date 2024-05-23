using AdmxParser.Contracts;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AdmxParser.Models
{
    public class Category : AdmxData, IHasNameAttribute, ILocalizable
    {
        protected Category(AdmxContent parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            _name = sourceElement.Attribute("name")?.Value;
            _displayName = sourceElement.Attribute("displayName")?.Value;

            var pathPrefix = Parent.PathPrefix;
            var nsManager = Parent.NamespaceManager;

            var parentCategoryElem = sourceElement.XPathSelectElement($"./{pathPrefix}parentCategory", nsManager);
            if (parentCategoryElem != default)
                _parentCategoryRef = parentCategoryElem.Attribute("ref")?.Value;
        }

        private readonly string _name;
        private readonly string _displayName;
        private readonly string _parentCategoryRef;

        public string Name => _name;
        public string DisplayName => _displayName;
        public string ParentCategoryRef => _parentCategoryRef;

        public bool HasParentCategory => !string.IsNullOrWhiteSpace(_parentCategoryRef);
    }

}
