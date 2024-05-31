using System.Xml.Linq;

namespace AdmxParser.Models.Admx
{
    /// <summary>
    /// Represents the target namespace in an ADMX file.
    /// </summary>
    public class TargetNamespace : AdmxData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TargetNamespace"/> class.
        /// </summary>
        /// <param name="parent">The parent <see cref="AdmxContent"/>.</param>
        /// <param name="sourceElement">The source <see cref="XElement"/>.</param>
        protected TargetNamespace(AdmxContent parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            _prefix = sourceElement.Attribute("prefix")?.Value;
            _namespace = sourceElement.Attribute("namespace")?.Value;
        }

        private readonly string _prefix;
        private readonly string _namespace;

        /// <summary>
        /// Gets the prefix of the target namespace.
        /// </summary>
        public string Prefix => _prefix;

        /// <summary>
        /// Gets the target namespace.
        /// </summary>
        public string Namespace => _namespace;
    }

}
