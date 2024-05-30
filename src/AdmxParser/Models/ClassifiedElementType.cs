namespace AdmxParser.Models
{
    /// <summary>
    /// Represents the type of the classified element data.
    /// </summary>
    public enum ClassifiedElementType : int
    {
        /// <summary>
        /// Represents an unknown element.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Represents an enum element.
        /// </summary>
        Enum,

        /// <summary>
        /// Represents a boolean element.
        /// </summary>
        Boolean,

        /// <summary>
        /// Represents a decimal element.
        /// </summary>
        Decimal,

        /// <summary>
        /// Represents a text element.
        /// </summary>
        Text,

        /// <summary>
        /// Represents a list of elements.
        /// </summary>
        List,

        /// <summary>
        /// Represents a multi-text element.
        /// </summary>
        MultiText,
    }
}
