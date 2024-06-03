using System.Xml.Linq;

namespace AdmxParser.Models.Adml
{
    /// <summary>
    /// Represents a presentation control entity.
    /// </summary>
    public class MultiTextBoxPresentation : AdmlPresentationControl
    {
        /// <summary>
        /// Creates a new instance of the <see cref="MultiTextBoxPresentation"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent resource of the entity.
        /// </param>
        /// <param name="sourceElement">
        /// The source element of the entity.
        /// </param>
        protected MultiTextBoxPresentation(AdmlResource parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            _showAsDialog = sourceElement.Attribute("showAsDialog")?.Value;
            _defaultHeight = sourceElement.Attribute("defaultHeight")?.Value;
        }

        private readonly string _showAsDialog;
        private readonly string _defaultHeight;

        /// <summary>
        /// Gets the value indicating whether the entity should be shown as a dialog.
        /// </summary>
        public string ShowAsDialog => _showAsDialog;
        
        /// <summary>
        /// Gets the default height of the entity.
        /// </summary>
        public string DefaultHeight => _defaultHeight;
    }
}
