using System.Xml.Linq;

namespace AdmxParser.Contracts
{
    /// <summary>
    /// Represents the interface for ADMX data.
    /// </summary>
    public interface IAdmxData
    {
        /// <summary>
        /// Gets the parent AdmxContent.
        /// </summary>
        AdmxContent Parent { get; }

        /// <summary>
        /// Gets the source XElement.
        /// </summary>
        XElement SourceElement { get; }
    }

}
