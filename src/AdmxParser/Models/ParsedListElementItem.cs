namespace AdmxParser.Models
{
    /// <summary>
    /// Represents a parsed list element item.
    /// </summary>
    public sealed class ParsedListElementItem : ParsedPolicyElementItem
    {
        /// <summary>
        /// Gets or sets the client extension.
        /// </summary>
        public string ClientExtension { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether the list element item is additive.
        /// </summary>
        public bool Additive { get; internal set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the list element item is expandable.
        /// </summary>
        public bool Expandable { get; internal set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the list element item has an explicit value.
        /// </summary>
        public bool ExplicitValue { get; internal set; } = false;
    }
}
