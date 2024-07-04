namespace AdmxParser.Models
{
    /// <summary>
    /// Represents a parsed policy element item.
    /// </summary>
    public abstract class ParsedPolicyElementItem
    {
        /// <summary>
        /// Gets or sets the ID of the policy element item.
        /// </summary>
        public string Id { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets or sets the parsed registry element item associated with the policy element item.
        /// </summary>
        public ParsedRegistryElementItem Item { get; internal set; } = default;
    }
}
