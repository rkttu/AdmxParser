namespace AdmxParser.Contracts
{
    /// <summary>
    /// Represents a localizable entity.
    /// </summary>
    public interface ILocalizable
    {
        /// <summary>
        /// Gets the display name of the entity.
        /// </summary>
        string DisplayName { get; }
    }
}
