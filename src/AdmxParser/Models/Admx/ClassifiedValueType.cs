namespace AdmxParser.Models.Admx
{
    /// <summary>
    /// Represents a classified value type.
    /// </summary>
    public enum ClassifiedValueType : int
    {
        /// <summary>
        /// Represents an unknown value.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Represents a delete value.
        /// </summary>
        Delete,

        /// <summary>
        /// Represents a string value.
        /// </summary>
        String,

        /// <summary>
        /// Represents a decimal value.
        /// </summary>
        Decimal
    }

}
