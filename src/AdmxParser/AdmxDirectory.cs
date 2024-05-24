using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AdmxParser
{
    /// <summary>
    /// Represents a directory containing ADMX files.
    /// </summary>
    public class AdmxDirectory
    {
        /// <summary>
        /// Gets the system policy definitions directory.
        /// </summary>
        /// <returns>The system policy definitions directory.</returns>
        public static AdmxDirectory GetSystemPolicyDefinitions()
            => new AdmxDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "PolicyDefinitions"));

        /// <summary>
        /// Initializes a new instance of the <see cref="AdmxDirectory"/> class.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        public AdmxDirectory(string directoryPath)
        {
            Helpers.EnsureDirectoryExists(directoryPath);
            _directoryPath = directoryPath;
            _loaded = false;
            _availableLanguages = new Dictionary<string, string>();
            _loadedAdmxContents = new List<AdmxContent>();
            _availableLanguagesReadOnly = new ReadOnlyDictionary<string, string>(_availableLanguages);
            _loadedAdmxContentsReadOnly = new ReadOnlyCollection<AdmxContent>(_loadedAdmxContents);
        }

        private readonly string _directoryPath;
        private bool _loaded;
        private readonly Dictionary<string, string> _availableLanguages;
        private readonly List<AdmxContent> _loadedAdmxContents;
        private readonly IReadOnlyDictionary<string, string> _availableLanguagesReadOnly;
        private readonly IReadOnlyList<AdmxContent> _loadedAdmxContentsReadOnly;

        /// <summary>
        /// Gets the directory path.
        /// </summary>
        public string DirectoryPath => _directoryPath;

        /// <summary>
        /// Gets a value indicating whether the contents are loaded.
        /// </summary>
        public bool Loaded => _loaded;

        /// <summary>
        /// Gets the available languages.
        /// </summary>
        public IReadOnlyDictionary<string, string> AvailableLanguages => _availableLanguagesReadOnly;

        /// <summary>
        /// Gets the loaded ADMX contents.
        /// </summary>
        public IReadOnlyList<AdmxContent> LoadedAdmxContents => _loadedAdmxContentsReadOnly;

        /// <summary>
        /// Loads the ADMX contents asynchronously.
        /// </summary>
        /// <param name="loadAdml">Whether to load ADML files.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<bool> LoadAsync(bool loadAdml = true, CancellationToken cancellationToken = default)
        {
            if (_loaded)
                throw new InvalidOperationException("Already contents loaded.");

            Helpers.EnsureDirectoryExists(_directoryPath);

            foreach (var eachAdmxFile in Helpers.SafeEnumerateFiles(_directoryPath, "*.admx", SearchOption.TopDirectoryOnly))
            {
                if (cancellationToken.IsCancellationRequested)
                    return false;

                var eachAdmlFiles = new Dictionary<string, AdmlResource>();
                var eachAdmlResources = new Dictionary<string, string>();
                var eachAdmxContent = new AdmxContent(eachAdmxFile);
                await eachAdmxContent.LoadAsync(loadAdml, cancellationToken).ConfigureAwait(false);
                _loadedAdmxContents.Add(eachAdmxContent);
            }

            _loaded = true;
            return true;
        }
    }

}
