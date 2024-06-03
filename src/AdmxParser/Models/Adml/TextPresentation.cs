using System.Xml.Linq;

namespace AdmxParser.Models.Adml
{
    /// <summary>
    /// Represents a text presentation entity.
    /// </summary>
    public class TextPresentation : AdmlPresentationControl
    {
        /// <summary>
        /// Creates a new instance of the <see cref="TextPresentation"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent resource of the entity.
        /// </param>
        /// <param name="sourceElement">
        /// The source element of the entity.
        /// </param>
        protected TextPresentation(AdmlResource parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            _innerText = sourceElement.Value;
        }

        private readonly string _innerText;

        /// <summary>
        /// Gets the inner text of the entity.
        /// </summary>
        public string InnerText => _innerText;
    }
}
