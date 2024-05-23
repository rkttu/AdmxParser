using System.Xml.Linq;

namespace AdmxParser.Models
{
    public class TargetNamespace : AdmxData
    {
        protected TargetNamespace(AdmxContent parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            _prefix = sourceElement.Attribute("prefix")?.Value;
            _namespace = sourceElement.Attribute("namespace")?.Value;
        }

        private readonly string _prefix;
        private readonly string _namespace;

        public string Prefix => _prefix;
        public string Namespace => _namespace;
    }

}
