using AdmxParser.Contracts;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AdmxParser.Models
{
    public class EnumItem : AdmxData, ILocalizable
    {
        protected EnumItem(AdmxContent parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            _displayName = sourceElement.Attribute("displayName")?.Value;

            var pathPrefix = Parent.PathPrefix;
            var nsManager = Parent.NamespaceManager;

            var valueElem = sourceElement.XPathSelectElement($"./{pathPrefix}value", nsManager);
            if (valueElem != default)
                _value = Parent.CreateAdmxData<PolicyValue>(valueElem.Elements().First());
        }

        private readonly string _displayName;
        private readonly PolicyValue _value;

        public string DisplayName => _displayName;
        public PolicyValue Value => _value;
    }

}
