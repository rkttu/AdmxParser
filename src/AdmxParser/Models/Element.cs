using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AdmxParser.Models
{
    /// <summary>
    /// Represents an element in the ADMX file.
    /// </summary>
    public class Element : AdmxData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Element"/> class.
        /// </summary>
        /// <param name="parent">The parent ADMX content.</param>
        /// <param name="sourceElement">The source XML element.</param>
        protected Element(AdmxContent parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            _elementType = sourceElement.Name.LocalName;
            _id = sourceElement.Attribute("id")?.Value;
            _key = sourceElement.Attribute("key")?.Value;
            _valueName = sourceElement.Attribute("valueName")?.Value;
            _required = sourceElement.Attribute("required")?.Value;
            _minValue = sourceElement.Attribute("minValue")?.Value;
            _maxValue = sourceElement.Attribute("maxValue")?.Value;
            _additive = sourceElement.Attribute("additive")?.Value;

            _enumItems = new List<EnumItem>();
            _enumItemsReadOnly = new ReadOnlyCollection<EnumItem>(_enumItems);

            var pathPrefix = Parent.PathPrefix;
            var nsManager = Parent.NamespaceManager;

            switch (_elementType?.ToUpperInvariant())
            {
                case "ENUM":
                    _classifiedElementType = ClassifiedElementType.Enum;
                    break;

                case "BOOLEAN":
                    _classifiedElementType = ClassifiedElementType.Boolean;
                    break;

                case "DECIMAL":
                    _classifiedElementType = ClassifiedElementType.Decimal;
                    break;

                case "TEXT":
                    _classifiedElementType = ClassifiedElementType.Text;
                    break;

                case "LIST":
                    _classifiedElementType = ClassifiedElementType.List;
                    break;

                default:
                    _classifiedElementType = ClassifiedElementType.Unknown;
                    break;
            }

            if (ClassifiedElementType == ClassifiedElementType.Enum)
            {
                var itemElems = sourceElement.XPathSelectElements($"./{pathPrefix}item", nsManager);
                foreach (var eachItemElem in itemElems)
                    _enumItems.Add(Parent.CreateAdmxData<EnumItem>(eachItemElem));
            }
            
            if (ClassifiedElementType == ClassifiedElementType.Boolean)
            {
                var trueValueElem = sourceElement.XPathSelectElement($"./{pathPrefix}trueValue", nsManager);
                if (trueValueElem != default)
                    _trueValue = Parent.CreateAdmxData<PolicyValue>(trueValueElem.Elements().First());

                var falseValueElem = sourceElement.XPathSelectElement($"./{pathPrefix}falseValue", nsManager);
                if (falseValueElem != default)
                    _falseValue = Parent.CreateAdmxData<PolicyValue>(falseValueElem.Elements().First());
            }
        }

        private readonly string _elementType;
        private readonly string _id;
        private readonly string _key;
        private readonly string _valueName;
        private readonly string _required;
        private readonly string _minValue;
        private readonly string _maxValue;
        private readonly string _additive;

        private readonly ClassifiedElementType _classifiedElementType;
        private readonly List<EnumItem> _enumItems;
        private readonly IReadOnlyList<EnumItem> _enumItemsReadOnly;
        private readonly PolicyValue _trueValue;
        private readonly PolicyValue _falseValue;

        /// <summary>
        /// Gets the type of the element.
        /// </summary>
        public string ElementType => _elementType;

        /// <summary>
        /// Gets the ID of the element.
        /// </summary>
        public string Id => _id;

        /// <summary>
        /// Gets the key of the element.
        /// </summary>
        public string Key => _key;

        /// <summary>
        /// Gets the value name of the element.
        /// </summary>
        public string ValueName => _valueName;

        /// <summary>
        /// Gets the required attribute of the element.
        /// </summary>
        public string Required => _required;

        /// <summary>
        /// Gets the minimum value of the element.
        /// </summary>
        public string MinValue => _minValue;

        /// <summary>
        /// Gets the maximum value of the element.
        /// </summary>
        public string MaxValue => _maxValue;

        /// <summary>
        /// Gets the additive attribute of the element.
        /// </summary>
        public string Additive => _additive;

        /// <summary>
        /// Gets the list of enum items for the element.
        /// </summary>
        public IReadOnlyList<EnumItem> EnumItems => _enumItemsReadOnly;

        /// <summary>
        /// Gets the true value for the element.
        /// </summary>
        public PolicyValue TrueValue => _trueValue;

        /// <summary>
        /// Gets the false value for the element.
        /// </summary>
        public PolicyValue FalseValue => _falseValue;

        /// <summary>
        /// Gets the classified type for the element.
        /// </summary>
        public ClassifiedElementType ClassifiedElementType => _classifiedElementType;
    }
}
