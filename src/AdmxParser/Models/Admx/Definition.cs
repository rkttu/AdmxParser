using AdmxParser.Contracts;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AdmxParser.Models.Admx
{
    /// <summary>
    /// Represents a definition in the ADMX file.
    /// </summary>
    public class Definition : AdmxData, IHasNameAttribute, ILocalizable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Definition"/> class.
        /// </summary>
        /// <param name="parent">The parent ADMX content.</param>
        /// <param name="sourceElement">The source XML element.</param>
        protected Definition(AdmxContent parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            _name = sourceElement.Attribute("name")?.Value;
            _displayName = sourceElement.Attribute("displayName")?.Value;
            _andReferences = new List<string>();
            _orReferences = new List<string>();

            var pathPrefix = Parent.PathPrefix;
            var nsManager = Parent.NamespaceManager;

            var andReferenceElems = sourceElement.XPathSelectElements($"./{pathPrefix}reference", nsManager);
            foreach (var eachReferenceElem in andReferenceElems)
            {
                var refExpression = eachReferenceElem.Attribute("ref")?.Value;
                if (refExpression == default)
                    continue;
                _andReferences.Add(refExpression);
            }

            var orReferenceElems = sourceElement.XPathSelectElements($"./{pathPrefix}reference", nsManager);
            foreach (var eachReferenceElem in orReferenceElems)
            {
                var refExpression = eachReferenceElem.Attribute("ref")?.Value;
                if (refExpression == default)
                    continue;
                _orReferences.Add(refExpression);
            }
        }

        private readonly string _name;
        private readonly string _displayName;
        private readonly List<string> _andReferences;
        private readonly List<string> _orReferences;

        /// <summary>
        /// Gets the name of the definition.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Gets the display name of the definition.
        /// </summary>
        public string DisplayName => _displayName;

        /// <summary>
        /// Gets the list of AND references of the definition.
        /// </summary>
        public IReadOnlyList<string> AndReferences => _andReferences;

        /// <summary>
        /// Gets the list of OR references of the definition.
        /// </summary>
        public IReadOnlyList<string> OrReferences => _orReferences;
    }

}
