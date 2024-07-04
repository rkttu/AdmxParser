namespace AdmxParser.Models
{
    /// <summary>
    /// Represents a parsed registry remove item.
    /// </summary>
    public sealed class ParsedRegistryRemoveItem : ParsedRegistryItemBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether the remove item is a prefix.
        /// </summary>
        public bool IsPrefix { get; internal set; }
    }
}
