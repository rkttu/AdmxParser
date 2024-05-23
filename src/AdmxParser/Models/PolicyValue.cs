using System;
using System.Xml.Linq;

namespace AdmxParser.Models
{
    public class PolicyValue : AdmxData
    {
        protected PolicyValue(AdmxContent parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            _valueType = sourceElement.Name.LocalName;

            switch (_valueType)
            {
                case "delete":
                    _delete = true;
                    break;
                case "string":
                    _value = sourceElement.Value;
                    break;
                case "decimal":
                    _value = sourceElement.Attribute("value")?.Value;
                    break;
                default:
                    throw new Exception($"Unsupported policy value type `{_valueType}`.");
            }
        }

        private readonly string _valueType;
        private readonly string _value;
        private readonly bool _delete;

        public string ValueType => _valueType;
        public string Value => _value;
        public bool Delete => _delete;
    }

}
