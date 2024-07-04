using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AdmxParser.Models
{
    /// <summary>
    /// Represents a collection of parsed policy definition items.
    /// </summary>
    public sealed class ParsedPolicyDefinitionItemCollection : KeyedCollection<string, ParsedPolicyDefinitionItem>
    {
        /// <summary>
        /// Gets the key for the specified item.
        /// </summary>
        /// <param name="item">The item to get the key for.</param>
        /// <returns>The key for the item.</returns>
        protected override string GetKeyForItem(ParsedPolicyDefinitionItem item)
        {
            return item.Namespace;
        }

        /// <summary>
        /// Finds the parsed policy definition items with the specified prefix.
        /// </summary>
        /// <param name="prefix">The prefix to search for.</param>
        /// <returns>An enumerable collection of parsed policy definition items.</returns>
        public IEnumerable<ParsedPolicyDefinitionItem> FindByPrefix(string prefix)
        {
            foreach (var eachItem in Items)
                if (string.Equals(prefix, eachItem.Prefix, StringComparison.Ordinal))
                    yield return eachItem;
        }
    }

}
