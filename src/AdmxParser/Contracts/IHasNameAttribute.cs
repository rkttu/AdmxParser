namespace AdmxParser.Contracts
{
    /// <summary>
    /// Represents an interface that has a Name attribute.
    /// </summary>
    public interface IHasNameAttribute
    {
        /// <summary>
        /// Gets the name attribute.
        /// </summary>
        string Name { get; }
    }

}
