using System.Xml.Linq;

namespace AdmxParser.Models
{
    /// <summary>
    /// Represents a using namespace declaration in an ADMX file.
    /// </summary>
    public class UsingNamespace : AdmxData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsingNamespace"/> class.
        /// </summary>
        /// <param name="parent">The parent <see cref="AdmxContent"/>.</param>
        /// <param name="sourceElement">The source <see cref="XElement"/>.</param>
        protected UsingNamespace(AdmxContent parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            _prefix = sourceElement.Attribute("prefix")?.Value;
            _namespace = sourceElement.Attribute("namespace")?.Value;
        }

        private readonly string _prefix;
        private readonly string _namespace;

        /// <summary>
        /// Gets the prefix of the namespace.
        /// </summary>
        public string Prefix => _prefix;

        /// <summary>
        /// Gets the namespace.
        /// </summary>
        public string Namespace => _namespace;
    }

}
