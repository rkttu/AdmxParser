using System.Xml.Linq;

namespace AdmxParser.Models.Adml
{
    /// <summary>
    /// Represents a long decimal text box presentation entity.
    /// </summary>
    public class LongDecimalTextBoxPresentation : AdmlPresentationControl
    {
        /// <summary>
        /// Creates a new instance of the <see cref="LongDecimalTextBoxPresentation"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent resource of the entity.
        /// </param>
        /// <param name="sourceElement">
        /// The source element of the entity.
        /// </param>
        protected LongDecimalTextBoxPresentation(AdmlResource parent, XElement sourceElement) :
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
