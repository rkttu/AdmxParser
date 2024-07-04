namespace AdmxParser.Models
{
    /// <summary>
    /// Represents a parsed long decimal element item.
    /// </summary>
    public sealed class ParsedLongDecimalElementItem : ParsedPolicyElementItem
    {
        /// <summary>
        /// Gets or sets the default value of the long decimal element.
        /// </summary>
        public long? DefaultValue { get; internal set; }

        /// <summary>
        /// Gets or sets the client extension of the long decimal element.
        /// </summary>
        public string ClientExtension { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether the long decimal element is required.
        /// </summary>
        public bool Required { get; internal set; } = false;

        /// <summary>
        /// Gets or sets the minimum value of the long decimal element.
        /// </summary>
        public long MinValue { get; internal set; } = 0L;

        /// <summary>
        /// Gets or sets the maximum value of the long decimal element.
        /// </summary>
        public long MaxValue { get; internal set; } = 9999L;

        /// <summary>
        /// Gets or sets a value indicating whether the long decimal element should be stored as text.
        /// </summary>
        public bool StoreAsText { get; internal set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the long decimal element is soft.
        /// </summary>
        public bool Soft { get; internal set; } = false;
    }
}
