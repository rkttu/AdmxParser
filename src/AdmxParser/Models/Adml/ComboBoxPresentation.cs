using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AdmxParser.Models.Adml
{
    /// <summary>
    /// Represents a combo box presentation entity.
    /// </summary>
    public class ComboBoxPresentation : AdmlPresentationControl
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ComboBoxPresentation"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent resource of the entity.
        /// </param>
        /// <param name="sourceElement">
        /// The source element of the entity.
        /// </param>
        protected ComboBoxPresentation(AdmlResource parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            _noSort = sourceElement.Attribute("noSort")?.Value;

            var pathPrefix = Parent.PathPrefix;
            var nsManager = Parent.NamespaceManager;

            var labelElem = sourceElement.XPathSelectElement($"./{pathPrefix}label", nsManager);
            if (labelElem != default)
                _label = labelElem.Value;

            var defaultElem = sourceElement.XPathSelectElement($"./{pathPrefix}default", nsManager);
            if (defaultElem != default)
                _default = defaultElem.Value;

            var suggestionsList = new List<string>();
            foreach (var eachSuggesstionElem in sourceElement.XPathSelectElements($"./{pathPrefix}suggestion", nsManager))
            {
                var suggestion = eachSuggesstionElem.Value;
                suggestionsList.Add(suggestion);
            }

            _suggestions = suggestionsList.ToArray();
        }

        private readonly string _noSort;
        private readonly string _label;
        private readonly string _default;
        private readonly string[] _suggestions;

        /// <summary>
        /// Gets the whether the entity is sorted or not.
        /// </summary>
        public string NoSort => _noSort;

        /// <summary>
        /// Gets the label of the entity.
        /// </summary>
        public string Label => _label;

        /// <summary>
        /// Gets the suggestions of the entity.
        /// </summary>
        public IReadOnlyList<string> Suggestions => _suggestions;
    }
}
