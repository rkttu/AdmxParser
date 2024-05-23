using System.Xml.Linq;

namespace AdmxParser.Models
{
    public class SupersededAdm : AdmxData
    {
        protected SupersededAdm(AdmxContent parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            _fileName = sourceElement.Attribute("fileName")?.Value;
        }

        private readonly string _fileName;

        public string FileName => _fileName;
    }

}
