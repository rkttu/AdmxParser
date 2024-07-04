using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AdmxParser.Models
{
    /// <summary>
    /// Represents a parsed policy definition item.
    /// </summary>
    public sealed class ParsedPolicyDefinitionItem
    {
        /// <summary>
        /// Gets or sets the namespace of the policy definition item.
        /// </summary>
        public string Namespace { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets or sets the prefix of the policy definition item.
        /// </summary>
        public string Prefix { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of parsed policy items.
        /// </summary>
        public ParsedPolicyItemCollection Policies { get; internal set; } = new ParsedPolicyItemCollection();

        /// <summary>
        /// Gets or sets the base string table of the policy definition item.
        /// </summary>
        public IReadOnlyDictionary<string, string> BaseStringTable { get; internal set; } = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
    }
}
