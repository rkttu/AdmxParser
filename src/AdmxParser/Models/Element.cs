using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AdmxParser.Models
{
    public class Element : AdmxData
    {
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

        public string ElementType => _elementType;
        public string Id => _id;
        public string Key => _key;
        public string ValueName => _valueName;
        public string Required => _required;
        public string MinValue => _minValue;
        public string MaxValue => _maxValue;
        public IReadOnlyList<EnumItem> EnumItems => _enumItemsReadOnly;
    }

}
