using System.Collections.Generic;

namespace AdmxParser.Models
{
    /// <summary>
    /// Represents a parsed enumeration element item.
    /// </summary>
    public sealed class ParsedEnumerationElementItem : ParsedPolicyElementItem
    {
        /// <summary>
        /// Gets or sets the default value of the enumeration element.
        /// </summary>
        public object DefaultValue { get; internal set; }

        /// <summary>
        /// Gets or sets the client extension of the enumeration element.
        /// </summary>
        public string ClientExtension { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether the enumeration element is required.
        /// </summary>
        public bool Required { get; internal set; } = false;

        /// <summary>
        /// Gets or sets the list of enumeration member items.
        /// </summary>
        public List<ParsedEnumerationMemberItem> EnumMemberItems { get; internal set; } = new List<ParsedEnumerationMemberItem>();
    }
}
