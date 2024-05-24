using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AdmxParser.Models
{
    /// <summary>
    /// Represents the SupportedOn class that contains information about the supported products and definitions.
    /// </summary>
    public class SupportedOn : AdmxData
    {
        /// <summary>
        /// Initializes a new instance of the SupportedOn class.
        /// </summary>
        /// <param name="parent">The parent AdmxContent.</param>
        /// <param name="sourceElement">The source XElement.</param>
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

        /// <summary>
        /// Gets the list of supported products.
        /// </summary>
        public IReadOnlyList<Product> Products => _products;

        /// <summary>
        /// Gets the list of definitions.
        /// </summary>
        public IReadOnlyList<Definition> Definitions => _definitions;
    }

}
