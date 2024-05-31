namespace AdmxParser.Contracts
{
    /// <summary>
    /// Represents an entity with a reference ID.
    /// </summary>
    public interface IHasReferenceId
    {
        /// <summary>
        /// Gets the reference ID of the entity.
        /// </summary>
        string RefId { get; }
    }
}
