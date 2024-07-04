namespace AdmxParser.Models
{
    /// <summary>
    /// Represents a parsed fixed registry item.
    /// </summary>
    public sealed class ParsedRegistryFixedItem : ParsedRegistryItemBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether the registry item should be deleted.
        /// </summary>
        public bool Delete { get; internal set; }

        /// <summary>
        /// Gets or sets the value of the registry item.
        /// </summary>
        public object Value { get; internal set; }
    }
}
