using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace AdmxParser
{
    /// <summary>
    /// Represents a Windows language.
    /// </summary>
    public sealed class WindowsLanguage
    {
        /// <summary>
        /// Static constructor.
        /// </summary>
        static WindowsLanguage()
        {
#pragma warning disable CS0618
            _windowsLanguages = new List<WindowsLanguage>
            {
                Arabic_SaudiArabia,
                Basque_Basque,
                Bulgarian_Bulgaria,
                Catalan_Spain,
                Chinese_Traditional_HongKongSAR,
                Chinese_Simplified_China,
                Chinese_Traditional_Taiwan,
                Croatian_Croatia,
                Czech_CzechRepublic,
                Danish_Denmark,
                Dutch_Netherlands,
                English_UnitedStates,
                English_UnitedKingdom,
                Estonian_Estonia,
                Finnish_Finland,
                French_Canada,
                French_France,
                Galician_Spain,
                German_Germany,
                Greek_Greece,
                Hebrew_Israel,
                Hungarian_Hungary,
                Indonesian_Indonesia,
                Italian_Italy,
                Japanese_Japan,
                Korean_Korea,
                Latvian_Latvia,
                Lithuanian_Lithuania,
                Norwegian_Norway,
                Polish_Poland,
                Portuguese_Brazil,
                Portuguese_Portugal,
                Romanian_Romania,
                Russian_Russia,
                Serbian_Latin_SerbiaMontenegro,
                Serbian_Latin_Serbia,
                Slovak_Slovakia,
                Slovenian_Slovenia,
                Spanish_Mexico,
                Spanish_Spain,
                Swedish_Sweden,
                Thai_Thailand,
                Turkish_Turkiye,
                Ukrainian_Ukraine,
                Vietnamese_Vietnam,
            };
#pragma warning restore CS0618

            _windowsLanguageCollection = new Dictionary<string, WindowsLanguage>();
            _cultureInfoToWindowsLanguages = new Dictionary<CultureInfo, WindowsLanguage>();

            for (var i = 0; i < _windowsLanguages.Count; i++)
            {
                _cultureInfoToWindowsLanguages.Add(_windowsLanguages[i].CultureInfo, _windowsLanguages[i]);
                _windowsLanguageCollection.Add(_windowsLanguages[i].IetfLanguageTag, _windowsLanguages[i]);
            }

            _windowsLanguagesReadOnly =
                new ReadOnlyCollection<WindowsLanguage>(_windowsLanguages);
            _windowsLanguageCollectionReadOnly =
                new ReadOnlyDictionary<string, WindowsLanguage>(_windowsLanguageCollection);
            _cultureInfoToWindowsLanguagesReadOnly =
                new ReadOnlyDictionary<CultureInfo, WindowsLanguage>(_cultureInfoToWindowsLanguages);
        }

        /// <summary>
        /// A dictionary mapping IETF language tag to <see cref="WindowsLanguage"/>.
        /// </summary>
        public static IDictionary<string, WindowsLanguage> WindowsLanguageCollection =>
            _windowsLanguageCollectionReadOnly;

        private static readonly Dictionary<string, WindowsLanguage> _windowsLanguageCollection;
        private static readonly IDictionary<string, WindowsLanguage> _windowsLanguageCollectionReadOnly;

        /// <summary>
        /// A dictionary mapping <see cref="CultureInfo"/> to <see cref="WindowsLanguages"/>.
        /// </summary>
        public static IReadOnlyDictionary<CultureInfo, WindowsLanguage> CultureInfoToWindowsLanguages
            => _cultureInfoToWindowsLanguagesReadOnly;

        private static readonly Dictionary<CultureInfo, WindowsLanguage> _cultureInfoToWindowsLanguages;
        private static readonly IReadOnlyDictionary<CultureInfo, WindowsLanguage> _cultureInfoToWindowsLanguagesReadOnly;

        /// <summary>
        /// A list of <see cref="WindowsLanguage"/>.
        /// </summary>
        public static IReadOnlyList<WindowsLanguage> WindowsLanguages
            => _windowsLanguagesReadOnly;

        private static readonly List<WindowsLanguage> _windowsLanguages;
        private static readonly IReadOnlyList<WindowsLanguage> _windowsLanguagesReadOnly;

        /// <summary>
        /// Arabic (Saudi Arabia)
        /// </summary>
        public static readonly WindowsLanguage Arabic_SaudiArabia = new WindowsLanguage("Arabic", "Saudi Arabia", "ar", "SA", 0x0401, false);

        /// <summary>
        /// Basque (Basque)
        /// </summary>
        public static readonly WindowsLanguage Basque_Basque = new WindowsLanguage("Basque", "Basque", "eu", "ES", 0x042d, false);

        /// <summary>
        /// Bulgarian (Bulgaria)
        /// </summary>
        public static readonly WindowsLanguage Bulgarian_Bulgaria = new WindowsLanguage("Bulgarian", "Bulgaria", "bg", "BG", 0x0402, false);

        /// <summary>
        /// Catalan (Spain)
        /// </summary>
        public static readonly WindowsLanguage Catalan_Spain = new WindowsLanguage("Catalan", "Spain", "ca", "ES", 0x0403, false);

        /// <summary>
        /// Chinese (Simplified, China)
        /// </summary>
        /// <remarks>
        /// This language definition is deprecated. Use zh-CN instead.
        /// </remarks>
        [Obsolete("This language definition is deprecated. Use zh-TW instead.")]
        public static readonly WindowsLanguage Chinese_Traditional_HongKongSAR = new WindowsLanguage("Chinese", "Traditional, Hong Kong SAR", "zh", "HK", 0x0c04, true);

        /// <summary>
        /// Chinese (Simplified, China)
        /// </summary>
        public static readonly WindowsLanguage Chinese_Simplified_China = new WindowsLanguage("Chinese", "Simplified, China", "zh", "CN", 0x0804, false);

        /// <summary>
        /// Chinese (Traditional, Taiwan)
        /// </summary>
        public static readonly WindowsLanguage Chinese_Traditional_Taiwan = new WindowsLanguage("Chinese", "Traditional, Taiwan", "zh", "TW", 0x0404, false);

        /// <summary>
        /// Croatian (Croatia)
        /// </summary>
        public static readonly WindowsLanguage Croatian_Croatia = new WindowsLanguage("Croatian", "Croatia", "hr", "HR", 0x041a, false);

        /// <summary>
        /// Czech (Czech Republic)
        /// </summary>
        public static readonly WindowsLanguage Czech_CzechRepublic = new WindowsLanguage("Czech", "Czech Republic", "cs", "CZ", 0x0405, false);

        /// <summary>
        /// Danish (Denmark)
        /// </summary>
        public static readonly WindowsLanguage Danish_Denmark = new WindowsLanguage("Danish", "Denmark", "da", "DK", 0x0406, false);

        /// <summary>
        /// Dutch (Netherlands)
        /// </summary>
        public static readonly WindowsLanguage Dutch_Netherlands = new WindowsLanguage("Dutch", "Netherlands", "nl", "NL", 0x0413, false);

        /// <summary>
        /// English (United States)
        /// </summary>
        public static readonly WindowsLanguage English_UnitedStates = new WindowsLanguage("English", "United States", "en", "US", 0x0409, false);

        /// <summary>
        /// English (United Kingdom)
        /// </summary>
        public static readonly WindowsLanguage English_UnitedKingdom = new WindowsLanguage("English", "United Kingdom", "en", "GB", 0x0809, false);

        /// <summary>
        /// Estonian (Estonia)
        /// </summary>
        public static readonly WindowsLanguage Estonian_Estonia = new WindowsLanguage("Estonian", "Estonia", "et", "EE", 0x0425, false);

        /// <summary>
        /// Finnish (Finland)
        /// </summary>
        public static readonly WindowsLanguage Finnish_Finland = new WindowsLanguage("Finnish", "Finland", "fi", "FI", 0x040b, false);

        /// <summary>
        /// French (Canada)
        /// </summary>
        public static readonly WindowsLanguage French_Canada = new WindowsLanguage("French", "Canada", "fr", "CA", 0x0c0c, false);

        /// <summary>
        /// French (France)
        /// </summary>
        public static readonly WindowsLanguage French_France = new WindowsLanguage("French", "France", "fr", "FR", 0x040c, false);

        /// <summary>
        /// Galician (Spain)
        /// </summary>
        public static readonly WindowsLanguage Galician_Spain = new WindowsLanguage("Galician", "Spain", "gl", "ES", 0x0456, false);

        /// <summary>
        /// German (Germany)
        /// </summary>
        public static readonly WindowsLanguage German_Germany = new WindowsLanguage("German", "Germany", "de", "DE", 0x0407, false);

        /// <summary>
        /// Greek (Greece)
        /// </summary>
        public static readonly WindowsLanguage Greek_Greece = new WindowsLanguage("Greek", "Greece", "el", "GR", 0x0408, false);

        /// <summary>
        /// Hebrew (Israel)
        /// </summary>
        public static readonly WindowsLanguage Hebrew_Israel = new WindowsLanguage("Hebrew", "Israel", "he", "IL", 0x040d, false);

        /// <summary>
        /// Hungarian (Hungary)
        /// </summary>
        public static readonly WindowsLanguage Hungarian_Hungary = new WindowsLanguage("Hungarian", "Hungary", "hu", "HU", 0x040e, false);

        /// <summary>
        /// Indonesian (Indonesia)
        /// </summary>
        public static readonly WindowsLanguage Indonesian_Indonesia = new WindowsLanguage("Indonesian", "Indonesia", "id", "ID", 0x0421, false);

        /// <summary>
        /// Italian (Italy)
        /// </summary>
        public static readonly WindowsLanguage Italian_Italy = new WindowsLanguage("Italian", "Italy", "it", "IT", 0x0410, false);

        /// <summary>
        /// Japanese (Japan)
        /// </summary>
        public static readonly WindowsLanguage Japanese_Japan = new WindowsLanguage("Japanese", "Japan", "ja", "JP", 0x0411, false);
        
        /// <summary>
        /// Korean (Korea)
        /// </summary>
        public static readonly WindowsLanguage Korean_Korea = new WindowsLanguage("Korean", "Korea", "ko", "KR", 0x0412, false);

        /// <summary>
        /// Latvian (Latvia)
        /// </summary>
        public static readonly WindowsLanguage Latvian_Latvia = new WindowsLanguage("Latvian", "Latvia", "lv", "LV", 0x0426, false);

        /// <summary>
        /// Lithuanian (Lithuania)
        /// </summary>
        public static readonly WindowsLanguage Lithuanian_Lithuania = new WindowsLanguage("Lithuanian", "Lithuania", "lt", "LT", 0x0427, false);

        /// <summary>
        /// Norwegian (Norway)
        /// </summary>
        public static readonly WindowsLanguage Norwegian_Norway = new WindowsLanguage("Norwegian", "Norway", "nb", "NO", 0x0414, false);

        /// <summary>
        /// Polish (Poland)
        /// </summary>
        public static readonly WindowsLanguage Polish_Poland = new WindowsLanguage("Polish", "Poland", "pl", "PL", 0x0415, false);

        /// <summary>
        /// Portuguese (Brazil)
        /// </summary>
        public static readonly WindowsLanguage Portuguese_Brazil = new WindowsLanguage("Portuguese", "Brazil", "pt", "BR", 0x0416, false);

        /// <summary>
        /// Portuguese (Portugal)
        /// </summary>
        public static readonly WindowsLanguage Portuguese_Portugal = new WindowsLanguage("Portuguese", "Portugal", "pt", "PT", 0x0816, false);

        /// <summary>
        /// Romanian (Romania)
        /// </summary>
        public static readonly WindowsLanguage Romanian_Romania = new WindowsLanguage("Romanian", "Romania", "ro", "RO", 0x0418, false);

        /// <summary>
        /// Russian (Russia)
        /// </summary>
        public static readonly WindowsLanguage Russian_Russia = new WindowsLanguage("Russian", "Russia", "ru", "RU", 0x0419, false);

        /// <summary>
        /// Serbian (Cyrillic, Bosnia and Herzegovina)
        /// </summary>
        /// <remarks>
        /// This language definition is deprecated. Use sr-Cyrl-RS instead.
        /// </remarks>
        [Obsolete("This language definition is deprecated. Use sr-Latn-RS instead.")]
        public static readonly WindowsLanguage Serbian_Latin_SerbiaMontenegro = new WindowsLanguage("Serbian", "Latin, Serbia", "sr-Latn", "CS", 0x081a, true);

        /// <summary>
        /// Serbian (Latin, Serbia)
        /// </summary>
        public static readonly WindowsLanguage Serbian_Latin_Serbia = new WindowsLanguage("Serbian", "Latin, Serbia", "sr-Latn", "RS", 0x241A, false);

        /// <summary>
        /// Slovak (Slovakia)
        /// </summary>
        public static readonly WindowsLanguage Slovak_Slovakia = new WindowsLanguage("Slovak", "Slovakia", "sk", "SK", 0x041b, false);

        /// <summary>
        /// Slovenian (Slovenia)
        /// </summary>
        public static readonly WindowsLanguage Slovenian_Slovenia = new WindowsLanguage("Slovenian", "Slovenia", "sl", "SI", 0x0424, false);

        /// <summary>
        /// Spanish (Mexico)
        /// </summary>
        public static readonly WindowsLanguage Spanish_Mexico = new WindowsLanguage("Spanish", "Mexico", "es", "MX", 0x080a, false);

        /// <summary>
        /// Spanish (Spain)
        /// </summary>
        public static readonly WindowsLanguage Spanish_Spain = new WindowsLanguage("Spanish", "Spain", "es", "ES", 0x0c0a, false);

        /// <summary>
        /// Swedish (Sweden)
        /// </summary>
        public static readonly WindowsLanguage Swedish_Sweden = new WindowsLanguage("Swedish", "Sweden", "sv", "SE", 0x041d, false);

        /// <summary>
        /// Thai (Thailand)
        /// </summary>
        public static readonly WindowsLanguage Thai_Thailand = new WindowsLanguage("Thai", "Thailand", "th", "TH", 0x041e, false);

        /// <summary>
        /// Turkish (Turkey)
        /// </summary>
        public static readonly WindowsLanguage Turkish_Turkiye = new WindowsLanguage("Turkish", "Turkey", "tr", "TR", 0x041f, false);

        /// <summary>
        /// Ukrainian (Ukraine)
        /// </summary>
        public static readonly WindowsLanguage Ukrainian_Ukraine = new WindowsLanguage("Ukrainian", "Ukraine", "uk", "UA", 0x0422, false);

        /// <summary>
        /// Vietnamese (Vietnam)
        /// </summary>
        public static readonly WindowsLanguage Vietnamese_Vietnam = new WindowsLanguage("Vietnamese", "Vietnam", "vi", "VN", 0x042a, false);

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsLanguages"/> class.
        /// </summary>
        /// <param name="language">
        /// The language. (e.g. English)
        /// </param>
        /// <param name="region">
        /// The region. (e.g. United States)
        /// </param>
        /// <param name="languageTag">
        /// The language tag. (e.g. en)
        /// </param>
        /// <param name="regionTag">
        /// The region tag. (e.g. US)
        /// </param>
        /// <param name="languageCodeIdentifier">
        /// The language code identifier. (e.g. 1033)
        /// </param>
        /// <param name="isDeprecated">
        /// A value indicating whether the language is deprecated.
        /// </param>
        private WindowsLanguage(string language, string region, string languageTag, string regionTag, int languageCodeIdentifier, bool isDeprecated)
        {
            _language = language;
            _region = region;
            _languageTag = languageTag;
            _regionTag = regionTag;
            _languageCodeIdentifier = languageCodeIdentifier;
            _isDeprecated = isDeprecated;
        }

        private readonly string _language;
        private readonly string _region;
        private readonly string _languageTag;
        private readonly string _regionTag;
        private readonly int _languageCodeIdentifier;
        private readonly bool _isDeprecated;

        /// <summary>
        /// Gets the language.
        /// </summary>
        public string Language => _language;

        /// <summary>
        /// Gets the region.
        /// </summary>
        public string Region => _region;

        /// <summary>
        /// Gets the language tag. (e.g. en)
        /// </summary>
        public string LanguageTag => _languageTag;

        /// <summary>
        /// Gets the region tag. (e.g. US)
        /// </summary>
        public string RegionTag => _regionTag;

        /// <summary>
        /// Gets the IETF language tag. (e.g. en-US)
        /// </summary>
        public string IetfLanguageTag => $"{_languageTag}-{_regionTag}";
        
        /// <summary>
        /// Gets the language code identifier.
        /// </summary>
        public int LanguageCodeIdentifier => _languageCodeIdentifier;

        /// <summary>
        /// Gets the language code identifier in hexadecimal format.
        /// </summary>
        public string LanguageCodeIdentifierHex => _languageCodeIdentifier.ToString("X4");

        /// <summary>
        /// Gets the culture info.
        /// </summary>
        public CultureInfo CultureInfo => new CultureInfo(IetfLanguageTag);

        /// <summary>
        /// Gets a value indicating whether the language is deprecated.
        /// </summary>
        public bool IsDeprecated => _isDeprecated;

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// Converted string.
        /// </returns>
        public override string ToString()
            => $"{(_isDeprecated ? "(Deprecated) " : string.Empty)}{_language} ({_languageTag}) - {_languageTag}-{_regionTag}: {_languageCodeIdentifier.ToString("X4")}";
    }
}
