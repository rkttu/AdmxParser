using System.Xml.Linq;

namespace AdmxParser.Models
{
    public class Resources : AdmxData
    {
        protected Resources(AdmxContent parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            _minRequiredRevision = sourceElement.Attribute("minRequiredRevision")?.Value;
        }

        private readonly string _minRequiredRevision;

        public string MinimumRequiredRevision => _minRequiredRevision;
    }

}
