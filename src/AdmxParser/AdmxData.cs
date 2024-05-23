using AdmxParser.Contracts;
using System.Xml.Linq;

namespace AdmxParser
{
    public abstract class AdmxData : IAdmxData
    {
        protected AdmxData(AdmxContent parent, XElement sourceElement)
        {
            _parent = parent;
            _sourceElement = sourceElement;
        }

        private readonly AdmxContent _parent;
        private readonly XElement _sourceElement;

        public AdmxContent Parent => _parent;
        public XElement SourceElement => _sourceElement;
    }

}
