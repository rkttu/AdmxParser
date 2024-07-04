namespace AdmxParser.Models
{
    /// <summary>
    /// Represents a parsed decimal element item.
    /// </summary>
    public sealed class ParsedDecimalElementItem : ParsedPolicyElementItem
    {
        /// <summary>
        /// Gets or sets the default value of the decimal element.
        /// </summary>
        public int? DefaultValue { get; internal set; }

        /// <summary>
        /// Gets or sets the client extension of the decimal element.
        /// </summary>
        public string ClientExtension { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether the decimal element is required.
        /// </summary>
        public bool Required { get; internal set; } = false;

        /// <summary>
        /// Gets or sets the minimum value of the decimal element.
        /// </summary>
        public int MinValue { get; internal set; } = 0;

        /// <summary>
        /// Gets or sets the maximum value of the decimal element.
        /// </summary>
        public int MaxValue { get; internal set; } = 9999;

        /// <summary>
        /// Gets or sets a value indicating whether the decimal element should be stored as text.
        /// </summary>
        public bool StoreAsText { get; internal set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the decimal element is soft.
        /// </summary>
        public bool Soft { get; internal set; } = false;
    }
}
