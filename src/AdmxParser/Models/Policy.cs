using AdmxParser.Contracts;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AdmxParser.Models
{
    /// <summary>
    /// Represents a policy in the ADMX file.
    /// </summary>
    public class Policy : AdmxData, IHasNameAttribute, ILocalizable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Policy"/> class.
        /// </summary>
        /// <param name="parent">The parent ADMX content.</param>
        /// <param name="sourceElement">The source XML element.</param>
        protected Policy(AdmxContent parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            // Initialize policy properties
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

        /// <summary>
        /// Gets the name of the policy.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Gets the display name of the policy.
        /// </summary>
        public string DisplayName => _displayName;

        /// <summary>
        /// Gets the class of the policy.
        /// </summary>
        public string Class => _class;

        /// <summary>
        /// Gets the explanation text of the policy.
        /// </summary>
        public string ExplainText => _explainText;

        /// <summary>
        /// Gets the presentation of the policy.
        /// </summary>
        public string Presentation => _presentation;

        /// <summary>
        /// Gets the key of the policy.
        /// </summary>
        public string Key => _key;

        /// <summary>
        /// Gets the value name of the policy.
        /// </summary>
        public string ValueName => _valueName;

        /// <summary>
        /// Gets the reference to the parent category of the policy.
        /// </summary>
        public string ParentCategoryRef => _parentCategoryRef;

        /// <summary>
        /// Gets the reference to the supported operating system of the policy.
        /// </summary>
        public string SupportedOnRef => _supportedOnRef;

        /// <summary>
        /// Gets the enabled value of the policy.
        /// </summary>
        public PolicyValue EnabledValue => _enabledValue;

        /// <summary>
        /// Gets the disabled value of the policy.
        /// </summary>
        public PolicyValue DisabledValue => _disabledValue;

        /// <summary>
        /// Gets the read-only collection of elements associated with the policy.
        /// </summary>
        public IReadOnlyList<Element> Elements => _elementsReadOnly;
    }

}
