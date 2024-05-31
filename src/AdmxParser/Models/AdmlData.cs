using AdmxParser.Contracts;
using System.Xml.Linq;

namespace AdmxParser.Models
{
    /// <summary>
    /// Represents the base class for AdmlData.
    /// </summary>
    public abstract class AdmlData : IAdmlData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdmlData"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent AdmlResource.
        /// </param>
        /// <param name="sourceElement">
        /// The source XElement.
        /// </param>
        public AdmlData(AdmlResource parent, XElement sourceElement)
        {
            _parent = parent;
            _sourceElement = sourceElement;
        }

        private readonly AdmlResource _parent;
        private readonly XElement _sourceElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdmlData"/> class.
        /// </summary>
        public AdmlResource Parent => _parent;

        /// <summary>
        /// Gets the source XElement.
        /// </summary>
        public XElement SourceElement => _sourceElement;
    }

}
