using AdmxParser.Contracts;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AdmxParser.Models
{
    public class Policy : AdmxData, IHasNameAttribute, ILocalizable
    {
        protected Policy(AdmxContent parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            _name = sourceElement.Attribute("name")?.Value;
            _displayName = sourceElement.Attribute("displayName")?.Value;
            _class = sourceElement.Attribute("class")?.Value;
            _explainText = sourceElement.Attribute("explainText")?.Value;
            _presentation = sourceElement.Attribute("presentation")?.Value;
            _key = sourceElement.Attribute("key")?.Value;
            _valueName = sourceElement.Attribute("valueName")?.Value;

            _elements = new List<Element>();
            _elementsReadOnly = new ReadOnlyCollection<Element>(_elements);

            var pathPrefix = Parent.PathPrefix;
            var nsManager = Parent.NamespaceManager;

            var parentCategoryElem = sourceElement.XPathSelectElement($"./{pathPrefix}parentCategory", nsManager);
            if (parentCategoryElem != default)
                _parentCategoryRef = parentCategoryElem.Attribute("ref")?.Value;

            var supportedOnElem = sourceElement.XPathSelectElement($"./{pathPrefix}supportedOn", nsManager);
            if (supportedOnElem != default)
                _supportedOnRef = supportedOnElem.Attribute("ref")?.Value;

            var enabledValueElem = sourceElement.XPathSelectElement($"./{pathPrefix}enabledValue", nsManager);
            if (enabledValueElem != default)
                _enabledValue = Parent.CreateAdmxData<PolicyValue>(enabledValueElem.Elements().First());

            var disabledValueElem = sourceElement.XPathSelectElement($"./{pathPrefix}disabledValue", nsManager);
            if (disabledValueElem != default)
                _disabledValue = Parent.CreateAdmxData<PolicyValue>(disabledValueElem.Elements().First());

            var elementsElem = sourceElement.XPathSelectElement($"./{pathPrefix}elements", nsManager);
            if (elementsElem != default)
            {
                foreach (var eachElementElem in elementsElem.Elements())
                    _elements.Add(Parent.CreateAdmxData<Element>(eachElementElem));
            }
        }

        private readonly string _name;
        private readonly string _displayName;
        private readonly string _class;
        private readonly string _explainText;
        private readonly string _presentation;
        private readonly string _key;
        private readonly string _valueName;
        private readonly string _parentCategoryRef;
        private readonly string _supportedOnRef;
        private readonly PolicyValue _enabledValue;
        private readonly PolicyValue _disabledValue;

        private readonly List<Element> _elements;
        private readonly ReadOnlyCollection<Element> _elementsReadOnly;

        public string Name => _name;
        public string DisplayName => _displayName;
        public string Class => _class;
        public string ExplainText => _explainText;
        public string Presentation => _presentation;
        public string Key => _key;
        public string ValueName => _valueName;
        public string ParentCategoryRef => _parentCategoryRef;
        public string SupportedOnRef => _supportedOnRef;
        public PolicyValue EnabledValue => _enabledValue;
        public PolicyValue DisabledValue => _disabledValue;
        public IReadOnlyList<Element> Elements => _elementsReadOnly;
    }

}
