using System.Collections.Generic;

namespace AdmxParser.Models
{
    /// <summary>
    /// Represents a parsed boolean element item.
    /// </summary>
    public sealed class ParsedBooleanElementItem : ParsedPolicyElementItem
    {
        /// <summary>
        /// Gets or sets the default value of the boolean element.
        /// </summary>
        public bool? DefaultValue { get; internal set; }

        /// <summary>
        /// Gets or sets the value when the boolean element is true.
        /// </summary>
        public object TrueValue { get; internal set; }

        /// <summary>
        /// Gets or sets the value when the boolean element is false.
        /// </summary>
        public object FalseValue { get; internal set; }

        /// <summary>
        /// Gets or sets the client extension of the boolean element.
        /// </summary>
        public string ClientExtension { get; internal set; }

        /// <summary>
        /// Gets or sets the list of parsed registry fixed items when the boolean element is true.
        /// </summary>
        public List<ParsedRegistryFixedItem> TrueItems { get; internal set; } = new List<ParsedRegistryFixedItem>();

        /// <summary>
        /// Gets or sets the list of parsed registry fixed items when the boolean element is false.
        /// </summary>
        public List<ParsedRegistryFixedItem> FalseItems { get; internal set; } = new List<ParsedRegistryFixedItem>();
    }
}
