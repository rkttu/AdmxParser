using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            _enumItems = new List<EnumItem>();
            _enumItemsReadOnly = new ReadOnlyCollection<EnumItem>(_enumItems);

            var pathPrefix = Parent.PathPrefix;
            var nsManager = Parent.NamespaceManager;

            if (string.Equals("enum", _elementType, StringComparison.Ordinal))
            {
                var itemElems = sourceElement.XPathSelectElements($"./{pathPrefix}item", nsManager);
                foreach (var eachItemElem in itemElems)
                    _enumItems.Add(Parent.CreateAdmxData<EnumItem>(eachItemElem));
            }
        }

        private readonly string _elementType;
        private readonly string _id;
        private readonly string _key;
        private readonly string _valueName;
        private readonly string _required;
        private readonly string _minValue;
        private readonly string _maxValue;

        private readonly List<EnumItem> _enumItems;
        private readonly IReadOnlyList<EnumItem> _enumItemsReadOnly;

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
        /// Gets the list of enum items for the element.
        /// </summary>
        public IReadOnlyList<EnumItem> EnumItems => _enumItemsReadOnly;
    }

}
