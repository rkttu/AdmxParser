using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AdmxParser.Models.Admx
{
    /// <summary>
    /// Represents an item in an disabled list.
    /// </summary>
    public class DisabledListItem : AdmxData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisabledListItem"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent <see cref="AdmxContent"/>.
        /// </param>
        /// <param name="sourceElement">
        /// The source <see cref="XElement"/>.
        /// </param>
        protected DisabledListItem(AdmxContent parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            _key = sourceElement.Attribute("key")?.Value;
            _valueName = sourceElement.Attribute("value")?.Value;

            var pathPrefix = Parent.PathPrefix;
            var nsManager = Parent.NamespaceManager;

            var valueElem = sourceElement.XPathSelectElement($"./{pathPrefix}value", nsManager);
            if (valueElem != default)
                _value = Parent.CreateAdmxData<PolicyValue>(valueElem.Elements().First());
        }

        private readonly string _key;
        private readonly string _valueName;
        private readonly PolicyValue _value;

        /// <summary>
        /// Gets the key of the item.
        /// </summary>
        public string Key => _key;

        /// <summary>
        /// Gets the value name of the item.
        /// </summary>
        public string ValueName => _valueName;

        /// <summary>
        /// Gets the value of the item.
        /// </summary>
        public PolicyValue Value => _value;
    }

}
