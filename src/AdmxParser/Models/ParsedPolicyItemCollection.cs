using System.Collections.ObjectModel;

namespace AdmxParser.Models
{
    /// <summary>
    /// Represents a collection of parsed policy items.
    /// </summary>
    public sealed class ParsedPolicyItemCollection : KeyedCollection<string, ParsedPolicyItem>
    {
        /// <summary>
        /// Gets the key for the specified policy item.
        /// </summary>
        /// <param name="item">The policy item.</param>
        /// <returns>The key for the policy item.</returns>
        protected override string GetKeyForItem(ParsedPolicyItem item)
            => item.Name;
    }
}
