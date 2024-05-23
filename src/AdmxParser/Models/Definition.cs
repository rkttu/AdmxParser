using AdmxParser.Contracts;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AdmxParser.Models
{
    public class Definition : AdmxData, IHasNameAttribute, ILocalizable
    {
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

        public string Name => _name;
        public string DisplayName => _displayName;
        public IReadOnlyList<string> AndReferences => _andReferences;
        public IReadOnlyList<string> OrReferences => _orReferences;
    }

}
