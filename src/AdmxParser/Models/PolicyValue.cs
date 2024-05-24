using System;
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
    }

}
