namespace AdmxParser.Models
{
    /// <summary>
    /// Represents a parsed text element item.
    /// </summary>
    public sealed class ParsedTextElementItem : ParsedPolicyElementItem
    {
        /// <summary>
        /// Gets or sets the default value of the text element.
        /// </summary>
        public string DefaultValue { get; internal set; }

        /// <summary>
        /// Gets or sets the client extension of the text element.
        /// </summary>
        public string ClientExtension { get; internal set; }

        /// <summary>
        /// Gets or sets the maximum length of the text element.
        /// </summary>
        public int MaxLength { get; internal set; } = 1023;

        /// <summary>
        /// Gets or sets a value indicating whether the text element is required.
        /// </summary>
        public bool Required { get; internal set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the text element is expandable.
        /// </summary>
        public bool Expandable { get; internal set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the text element is soft.
        /// </summary>
        public bool Soft { get; internal set; } = false;
    }
}
