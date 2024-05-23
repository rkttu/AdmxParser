using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AdmxParser
{
    public class AdmxDirectory
    {
        public static AdmxDirectory GetSystemPolicyDefinitions()
            => new AdmxDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "PolicyDefinitions"));

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

        public string DirectoryPath => _directoryPath;
        public bool Loaded => _loaded;
        public IReadOnlyDictionary<string, string> AvailableLanguages => _availableLanguagesReadOnly;
        public IReadOnlyList<AdmxContent> LoadedAdmxContents => _loadedAdmxContentsReadOnly;

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
