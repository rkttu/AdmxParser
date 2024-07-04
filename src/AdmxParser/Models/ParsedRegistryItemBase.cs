namespace AdmxParser.Models
{
    /// <summary>
    /// Represents a base class for parsed registry items.
    /// </summary>
    public abstract class ParsedRegistryItemBase
    {
        /// <summary>
        /// Gets or sets the key of the registry item.
        /// </summary>
        public string Key { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets or sets the value name of the registry item.
        /// </summary>
        public string ValueName { get; internal set; } = string.Empty;
    }
}
