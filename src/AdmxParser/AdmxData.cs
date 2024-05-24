using AdmxParser.Contracts;
using System.Xml.Linq;

namespace AdmxParser
{
    /// <summary>
    /// Represents the base class for AdmxData.
    /// </summary>
    public abstract class AdmxData : IAdmxData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdmxData"/> class.
        /// </summary>
        /// <param name="parent">The parent AdmxContent.</param>
        /// <param name="sourceElement">The source XElement.</param>
        protected AdmxData(AdmxContent parent, XElement sourceElement)
        {
            _parent = parent;
            _sourceElement = sourceElement;
        }

        private readonly AdmxContent _parent;
        private readonly XElement _sourceElement;

        /// <summary>
        /// Gets the parent AdmxContent.
        /// </summary>
        public AdmxContent Parent => _parent;

        /// <summary>
        /// Gets the source XElement.
        /// </summary>
        public XElement SourceElement => _sourceElement;
    }

}
