using AdmxParser.Contracts;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AdmxParser.Models
{
    /// <summary>
    /// Represents a category in the ADMX file.
    /// </summary>
    public class Category : AdmxData, IHasNameAttribute, ILocalizable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Category"/> class.
        /// </summary>
        /// <param name="parent">The parent ADMX content.</param>
        /// <param name="sourceElement">The source XML element.</param>
        protected Category(AdmxContent parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            _name = sourceElement.Attribute("name")?.Value;
            _displayName = sourceElement.Attribute("displayName")?.Value;

            var pathPrefix = Parent.PathPrefix;
            var nsManager = Parent.NamespaceManager;

            var parentCategoryElem = sourceElement.XPathSelectElement($"./{pathPrefix}parentCategory", nsManager);
            if (parentCategoryElem != default)
                _parentCategoryRef = parentCategoryElem.Attribute("ref")?.Value;
        }

        private readonly string _name;
        private readonly string _displayName;
        private readonly string _parentCategoryRef;

        /// <summary>
        /// Gets the name of the category.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Gets the display name of the category.
        /// </summary>
        public string DisplayName => _displayName;

        /// <summary>
        /// Gets the reference to the parent category.
        /// </summary>
        public string ParentCategoryRef => _parentCategoryRef;

        /// <summary>
        /// Gets a value indicating whether the category has a parent category.
        /// </summary>
        public bool HasParentCategory => !string.IsNullOrWhiteSpace(_parentCategoryRef);
    }

}
