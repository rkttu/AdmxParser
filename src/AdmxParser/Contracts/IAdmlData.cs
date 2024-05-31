using System.Xml.Linq;

namespace AdmxParser.Contracts
{
    /// <summary>
    /// Represents the interface for ADML data.
    /// </summary>
    public interface IAdmlData
    {
        /// <summary>
        /// Gets the parent ADML resource.
        /// </summary>
        AdmlResource Parent { get; }

        /// <summary>
        /// Gets the source XElement.
        /// </summary>
        XElement SourceElement { get; }
    }
}
