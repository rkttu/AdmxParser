using System.Xml.Linq;

namespace AdmxParser.Models.Adml
{
    /// <summary>
    /// Represents a drop down list presentation entity.
    /// </summary>
    public class DropDownListPresentation : AdmlPresentationControl
    {
        /// <summary>
        /// Creates a new instance of the <see cref="CheckBoxPresentation"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent resource of the entity.
        /// </param>
        /// <param name="sourceElement">
        /// The source element of the entity.
        /// </param>
        protected DropDownListPresentation(AdmlResource parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            _noSort = sourceElement.Attribute("noSort")?.Value;
            _defaultItem = sourceElement.Attribute("defaultItem")?.Value;

            _label = sourceElement.Value;
        }

        private readonly string _noSort;
        private readonly string _defaultItem;
        private readonly string _label;

        /// <summary>
        /// Gets the whether the entity is sorted or not.
        /// </summary>
        public string NoSort => _noSort;

        /// <summary>
        /// Gets the default item of the entity.
        /// </summary>
        public string DefaultItem => _defaultItem;

        /// <summary>
        /// Gets the label of the entity.
        /// </summary>
        public string Label => _label;
    }
}
