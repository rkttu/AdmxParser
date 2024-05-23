using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AdmxParser.Models
{
    public class SupportedOn : AdmxData
    {
        protected SupportedOn(AdmxContent parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            _products = new List<Product>();
            _definitions = new List<Definition>();

            var pathPrefix = Parent.PathPrefix;
            var nsManager = Parent.NamespaceManager;

            var productElems = sourceElement.XPathSelectElements($"./{pathPrefix}products/{pathPrefix}product", nsManager);
            foreach (var eachProductElem in productElems)
                _products.Add(Parent.CreateAdmxData<Product>(eachProductElem));

            var definitionElems = sourceElement.XPathSelectElements($"./{pathPrefix}definitions/{pathPrefix}definition", nsManager);
            foreach (var eachDefinitionElem in definitionElems)
                _definitions.Add(Parent.CreateAdmxData<Definition>(eachDefinitionElem));
        }

        private readonly List<Product> _products;
        private readonly List<Definition> _definitions;

        public IReadOnlyList<Product> Products => _products;
        public IReadOnlyList<Definition> Definitions => _definitions;
    }

}
