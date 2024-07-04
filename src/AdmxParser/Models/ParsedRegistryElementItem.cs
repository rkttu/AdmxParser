using System;

namespace AdmxParser.Models
{
    /// <summary>
    /// Represents a parsed registry element item.
    /// </summary>
    public sealed class ParsedRegistryElementItem : ParsedRegistryItemBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether the registry element item is a prefix.
        /// </summary>
        public bool IsPrefix { get; internal set; }

        /// <summary>
        /// Gets or sets the value type of the registry element item.
        /// </summary>
        public Type ValueType { get; internal set; }
    }
}
