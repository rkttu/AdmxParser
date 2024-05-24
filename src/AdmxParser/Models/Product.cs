using AdmxParser.Contracts;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AdmxParser.Models
{
    /// <summary>
    /// Represents a product in the ADMX file.
    /// </summary>
    public class Product : AdmxData, IHasNameAttribute, ILocalizable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class.
        /// </summary>
        /// <param name="parent">The parent ADMX content.</param>
        /// <param name="sourceElement">The source XML element.</param>
        protected Product(AdmxContent parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            _name = sourceElement.Attribute("name")?.Value;
            _displayName = sourceElement.Attribute("displayName")?.Value;

            _majorVersions = new List<MajorVersion>();
            _majorVersionsReadOnly = new ReadOnlyCollection<MajorVersion>(_majorVersions);

            var pathPrefix = Parent.PathPrefix;
            var nsManager = Parent.NamespaceManager;

            var majorVersionElems = sourceElement.XPathSelectElements($"./{pathPrefix}majorVersion", nsManager);
            foreach (var eachMajorVersionElem in majorVersionElems)
                _majorVersions.Add(Parent.CreateAdmxData<MajorVersion>(eachMajorVersionElem));
        }

        /// <summary>
        /// Gets the name of the product.
        /// </summary>
        private readonly string _name;

        /// <summary>
        /// Gets the display name of the product.
        /// </summary>
        private readonly string _displayName;

        /// <summary>
        /// Gets the major versions of the product.
        /// </summary>
        private readonly List<MajorVersion> _majorVersions;

        /// <summary>
        /// Gets the read-only collection of major versions of the product.
        /// </summary>
        private readonly ReadOnlyCollection<MajorVersion> _majorVersionsReadOnly;

        /// <summary>
        /// Gets the name of the product.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Gets the display name of the product.
        /// </summary>
        public string DisplayName => _displayName;

        /// <summary>
        /// Gets the read-only list of major versions of the product.
        /// </summary>
        public IReadOnlyList<MajorVersion> MajorVersions => _majorVersionsReadOnly;
    }

}
