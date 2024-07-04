namespace AdmxParser.Models
{
    /// <summary>
    /// Represents a parsed multi-text element item.
    /// </summary>
    public sealed class ParsedMultiTextElementItem : ParsedPolicyElementItem
    {
        /// <summary>
        /// Gets or sets the client extension.
        /// </summary>
        public string ClientExtension { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether this item is required.
        /// </summary>
        public bool Required { get; internal set; } = false;

        /// <summary>
        /// Gets or sets the maximum length of the text.
        /// </summary>
        public int MaxLength { get; internal set; } = 1023;

        /// <summary>
        /// Gets or sets the maximum number of strings.
        /// </summary>
        public int MaxStrings { get; internal set; } = 0;

        /// <summary>
        /// Gets or sets a value indicating whether this item is soft.
        /// </summary>
        public bool Soft { get; internal set; } = false;
    }
}
