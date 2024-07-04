using System.Collections.ObjectModel;

namespace AdmxParser.Models
{
    /// <summary>
    /// Represents a collection of parsed policy element items.
    /// </summary>
    public sealed class ParsedElementItemCollection : KeyedCollection<string, ParsedPolicyElementItem>
    {
        /// <summary>
        /// Gets the key for the specified parsed policy element item.
        /// </summary>
        /// <param name="item">The parsed policy element item.</param>
        /// <returns>The key for the parsed policy element item.</returns>
        protected override string GetKeyForItem(ParsedPolicyElementItem item)
            => item.Id;
    }
}
