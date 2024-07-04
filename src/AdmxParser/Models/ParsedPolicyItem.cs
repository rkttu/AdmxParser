using System.Collections.Generic;

namespace AdmxParser.Models
{
    /// <summary>
    /// Represents a parsed policy item.
    /// </summary>
    public sealed class ParsedPolicyItem
    {
        /// <summary>
        /// Gets or sets the name of the policy item.
        /// </summary>
        public string Name { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets or sets the display name of the policy item.
        /// </summary>
        public string DisplayName { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets or sets the explanation text of the policy item.
        /// </summary>
        public string ExplainText { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the policy item is a machine policy.
        /// </summary>
        public bool IsMachinePolicy { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether the policy item is a user policy.
        /// </summary>
        public bool IsUserPolicy { get; internal set; }

        /// <summary>
        /// Gets or sets the list of enable items for the policy item.
        /// </summary>
        public List<ParsedRegistryFixedItem> EnableItems { get; internal set; } = new List<ParsedRegistryFixedItem>();

        /// <summary>
        /// Gets or sets the list of disable items for the policy item.
        /// </summary>
        public List<ParsedRegistryFixedItem> DisableItems { get; internal set; } = new List<ParsedRegistryFixedItem>();

        /// <summary>
        /// Gets or sets the collection of element items for the policy item.
        /// </summary>
        public ParsedElementItemCollection Elements { get; internal set; } = new ParsedElementItemCollection();

        /// <summary>
        /// Gets or sets the list of reset items for the policy item.
        /// </summary>
        public List<ParsedRegistryRemoveItem> ResetItems { get; internal set; } = new List<ParsedRegistryRemoveItem>();
    }

}
