namespace AdmxParser.Contracts
{
    /// <summary>
    /// Represents an explainable entity.
    /// </summary>
    public interface IPresentable
    {
        /// <summary>
        /// Gets the presentation of the entity.
        /// </summary>
        string Presentation { get;  }
    }
}
