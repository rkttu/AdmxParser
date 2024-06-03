using AdmxParser.Contracts;
using System.Xml.Linq;

namespace AdmxParser.Models.Adml
{
    /// <summary>
    /// Represents a presentation control entity.
    /// </summary>
    public abstract class AdmlPresentationControl : AdmlData, IHasReferenceId
    {
        /// <summary>
        /// Creates a new instance of the <see cref="AdmlPresentationControl"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent resource of the entity.
        /// </param>
        /// <param name="sourceElement">
        /// The source element of the entity.
        /// </param>
        protected AdmlPresentationControl(AdmlResource parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            _refId = sourceElement.Attribute("refId")?.Value;
        }

        private readonly string _refId;

        /// <summary>
        /// Creates a new instance of the <see cref="CheckBoxPresentation"/> class.
        /// </summary>
        public virtual string RefId => _refId;
    }
}
