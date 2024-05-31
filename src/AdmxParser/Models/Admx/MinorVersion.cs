using AdmxParser.Contracts;
using System.Xml.Linq;

namespace AdmxParser.Models.Admx
{
    /// <summary>
    /// Represents a minor version in the ADMX data.
    /// </summary>
    public class MinorVersion : AdmxData, IHasNameAttribute, ILocalizable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MinorVersion"/> class.
        /// </summary>
        /// <param name="parent">The parent ADMX content.</param>
        /// <param name="sourceElement">The source XML element.</param>
        protected MinorVersion(AdmxContent parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            _name = sourceElement.Attribute("name")?.Value;
            _displayName = sourceElement.Attribute("displayName")?.Value;
            _versionIndex = int.TryParse(sourceElement.Attribute("versionIndex")?.Value, out int versionIndex) ? versionIndex : default;
        }

        private readonly string _name;
        private readonly string _displayName;
        private readonly int? _versionIndex;

        /// <summary>
        /// Gets the name of the minor version.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Gets the display name of the minor version.
        /// </summary>
        public string DisplayName => _displayName;

        /// <summary>
        /// Gets the version index of the minor version.
        /// </summary>
        public int? VersionIndex => _versionIndex;
    }

}
