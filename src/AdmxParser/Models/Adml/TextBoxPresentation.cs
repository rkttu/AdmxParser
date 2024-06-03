using System.Xml.Linq;
using System.Xml.XPath;

namespace AdmxParser.Models.Adml
{
    /// <summary>
    /// Represents a text box presentation entity.
    /// </summary>
    public class TextBoxPresentation : AdmlPresentationControl
    {
        /// <summary>
        /// Creates a new instance of the <see cref="TextBoxPresentation"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent resource of the entity.
        /// </param>
        /// <param name="sourceElement">
        /// The source element of the entity.
        /// </param>
        protected TextBoxPresentation(AdmlResource parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            var pathPrefix = Parent.PathPrefix;
            var nsManager = Parent.NamespaceManager;

            var labelElem = sourceElement.XPathSelectElement($"./{pathPrefix}label", nsManager);
            if (labelElem != default)
                _label = labelElem.Value;

            var defaultValueElem = sourceElement.XPathSelectElement($"./{pathPrefix}defaultValue", nsManager);
            if (defaultValueElem != default)
                _defaultValue = defaultValueElem.Value;
        }

        private readonly string _label;
        private readonly string _defaultValue;

        /// <summary>
        /// Gets the label of the entity.
        /// </summary>
        public string Label => _label;

        /// <summary>
        /// Gets the default value of the entity.
        /// </summary>
        public string DefaultValue => _defaultValue;
    }
}
