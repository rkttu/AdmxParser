using AdmxParser.Contracts;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AdmxParser.Models.Admx
{
    /// <summary>
    /// Represents an item in an enumeration.
    /// </summary>
    public class EnumItem : AdmxData, ILocalizable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumItem"/> class.
        /// </summary>
        /// <param name="parent">The parent <see cref="AdmxContent"/>.</param>
        /// <param name="sourceElement">The source <see cref="XElement"/>.</param>
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

        /// <summary>
        /// Gets the display name of the enumeration item.
        /// </summary>
        public string DisplayName => _displayName;

        /// <summary>
        /// Gets the value of the enumeration item.
        /// </summary>
        public PolicyValue Value => _value;
    }

}
