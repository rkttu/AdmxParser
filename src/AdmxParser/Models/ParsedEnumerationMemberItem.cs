using System.Collections.Generic;

namespace AdmxParser.Models
{
    /// <summary>
    /// Represents a parsed enumeration member item.
    /// </summary>
    public sealed class ParsedEnumerationMemberItem
    {
        /// <summary>
        /// Gets or sets the value of the enumeration member.
        /// </summary>
        public object MemberValue { get; internal set; }

        /// <summary>
        /// Gets or sets the display name key of the enumeration member.
        /// </summary>
        public string DisplayNameKey { get; internal set; }

        /// <summary>
        /// Gets or sets the name of the enumeration member.
        /// </summary>
        public string MemberName { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of registry items associated with the enumeration member.
        /// </summary>
        public List<ParsedRegistryFixedItem> RegistryItems { get; internal set; } = new List<ParsedRegistryFixedItem>();
    }
}
