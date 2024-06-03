using System.Xml.Linq;

namespace AdmxParser.Models.Adml
{
    /// <summary>
    /// Represents a check box presentation entity.
    /// </summary>
    public class CheckBoxPresentation : AdmlPresentationControl
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
        protected CheckBoxPresentation(AdmlResource parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            _defaultChecked = sourceElement.Attribute("defaultChecked")?.Value;
            _value = sourceElement.Value;
        }

        private readonly string _defaultChecked;
        private readonly string _value;

        /// <summary>
        /// Gets the default checked value of the entity.
        /// </summary>
        public string DefaultChecked => _defaultChecked;

        /// <summary>
        /// Gets the value of the entity.
        /// </summary>
        public string Value => _value;
    }
}
