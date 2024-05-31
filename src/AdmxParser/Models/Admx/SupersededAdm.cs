using System.Xml.Linq;

namespace AdmxParser.Models.Admx
{
    /// <summary>
    /// Represents a superseded ADM file.
    /// </summary>
    public class SupersededAdm : AdmxData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SupersededAdm"/> class.
        /// </summary>
        /// <param name="parent">The parent <see cref="AdmxContent"/>.</param>
        /// <param name="sourceElement">The source <see cref="XElement"/>.</param>
        protected SupersededAdm(AdmxContent parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            _fileName = sourceElement.Attribute("fileName")?.Value;
        }

        private readonly string _fileName;

        /// <summary>
        /// Gets the file name.
        /// </summary>
        public string FileName => _fileName;
    }

}
