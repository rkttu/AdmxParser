namespace AdmxParser.Contracts
{
    /// <summary>
    /// Represents an explainable entity.
    /// </summary>
    public interface IExplainable
    {
        /// <summary>
        /// Gets the explanation of the entity.
        /// </summary>
        string ExplainText { get; }
    }
}
