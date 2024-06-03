using System.Xml.Linq;

namespace AdmxParser.Models.Adml
{
    /// <summary>
    /// Represents a decimal text box presentation entity.
    /// </summary>
    public class DecimalTextBoxPresentation : AdmlPresentationControl
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DecimalTextBoxPresentation"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent resource of the entity.
        /// </param>
        /// <param name="sourceElement">
        /// The source element of the entity.
        /// </param>
        protected DecimalTextBoxPresentation(AdmlResource parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            _defaultValue = sourceElement.Attribute("defaultValue")?.Value;
            _spin = sourceElement.Attribute("spin")?.Value;
            _spinStep = sourceElement.Attribute("spinStep")?.Value;
            _label = sourceElement.Value;
        }

        private readonly string _defaultValue;
        private readonly string _spin;
        private readonly string _spinStep;
        private readonly string _label;

        /// <summary>
        /// Gets the default value of the entity.
        /// </summary>
        public string DefaultValue => _defaultValue;

        /// <summary>
        /// Gets the whether spin is enabled or not.
        /// </summary>
        public string Spin => _spin;

        /// <summary>
        /// Gets the spin step of the entity.
        /// </summary>
        public string SpinStep => _spinStep;

        /// <summary>
        /// Gets the label of the entity.
        /// </summary>
        public string Label => _label;
    }
}
