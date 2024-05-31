using System.Xml.Linq;

namespace AdmxParser.Models.Admx
{
    /// <summary>
    /// Represents the resources in an ADMX file.
    /// </summary>
    public class Resources : AdmxData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Resources"/> class.
        /// </summary>
        /// <param name="parent">The parent ADMX content.</param>
        /// <param name="sourceElement">The source XML element.</param>
        protected Resources(AdmxContent parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            _minRequiredRevision = sourceElement.Attribute("minRequiredRevision")?.Value;
        }

        private readonly string _minRequiredRevision;

        /// <summary>
        /// Gets the minimum required revision.
        /// </summary>
        public string MinimumRequiredRevision => _minRequiredRevision;
    }

}
