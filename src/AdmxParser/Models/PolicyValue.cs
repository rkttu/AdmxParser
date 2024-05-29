using System.Xml.Linq;

namespace AdmxParser.Models
{
    /// <summary>
    /// Represents a policy value in the ADMX file.
    /// </summary>
    public class PolicyValue : AdmxData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyValue"/> class.
        /// </summary>
        /// <param name="parent">The parent ADMX content.</param>
        /// <param name="sourceElement">The source XML element.</param>
        protected PolicyValue(AdmxContent parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            _valueType = sourceElement.Name.LocalName;

            switch (_valueType.ToUpperInvariant())
            {
                case "DELETE":
                    _classifiedValueType = ClassifiedValueType.Delete;
                    _delete = true;
                    break;

                case "STRING":
                    _classifiedValueType = ClassifiedValueType.String;
                    _value = sourceElement.Value;
                    break;

                case "DECIMAL":
                    _classifiedValueType = ClassifiedValueType.Decimal;
                    _value = sourceElement.Attribute("value")?.Value;
                    break;

                default:
                    _classifiedValueType = ClassifiedValueType.Unknown;
                    _value = sourceElement?.Value;
                    break;
            }
        }

        private readonly string _valueType;
        private readonly string _value;
        private readonly bool _delete;

        private readonly ClassifiedValueType _classifiedValueType;

        /// <summary>
        /// Gets the value type of the policy value.
        /// </summary>
        public string ValueType => _valueType;

        /// <summary>
        /// Gets the value of the policy value.
        /// </summary>
        public string Value => _value;

        /// <summary>
        /// Gets a value indicating whether the policy value should be deleted.
        /// </summary>
        public bool Delete => _delete;

        /// <summary>
        /// Gets the classified value type of the policy value.
        /// </summary>
        public ClassifiedValueType ClassifiedValueType => _classifiedValueType;
    }
}
