using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdmxParser
{
    /// <summary>
    /// Represents a reference to an ADMX resource.
    /// </summary>
    public sealed class AdmxResourceReference
    {
        private static readonly Lazy<Regex> _expressionParserFactory = new Lazy<Regex>(() => new Regex(
            @"\$\((?<ResourceType>[^.]+)\.(?<ResourceKey>[^)]+)\)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase), false);

        /// <summary>
        /// Parses the specified expression and returns a collection of resource references.
        /// </summary>
        /// <param name="expression">
        /// The expression to parse.
        /// </param>
        /// <returns>
        /// A collection of resource references.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="expression"/> is <see langword="null"/>.
        /// </exception>
        public static IEnumerable<AdmxResourceReference> Parse(string expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return _expressionParserFactory.Value
                .Matches(expression)
                .Cast<Match>()
                .Select(match => new AdmxResourceReference(match.Index, match.Length, match.Groups["ResourceType"].Value, match.Groups["ResourceKey"].Value));
        }

        /// <summary>
        /// Interpolates the specified expression using the specified resources and target culture.
        /// </summary>
        /// <param name="expression">
        /// The expression to interpolate.
        /// </param>
        /// <param name="resources">
        /// The resources to use for interpolation.
        /// </param>
        /// <param name="targetCulture">
        /// The target culture.
        /// </param>
        /// <param name="allowFallbackToEnUs">
        /// Whether to allow fallback to en-US.
        /// </param>
        /// <returns>
        /// The interpolated expression.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="targetCulture"/> is <see langword="null"/>.
        /// </exception>
        public static string Interpolate(string expression, IReadOnlyDictionary<CultureInfo, AdmlResource> resources, CultureInfo targetCulture, bool allowFallbackToEnUs)
            => InterpolateInternal(expression, Parse(expression), resources, targetCulture, allowFallbackToEnUs);

        internal static string InterpolateInternal(string expression, IEnumerable<AdmxResourceReference> targets, IReadOnlyDictionary<CultureInfo, AdmlResource> resources, CultureInfo targetCulture, bool allowFallbackToEnUs)
        {
            if (targetCulture == null)
                throw new ArgumentNullException(nameof(targetCulture));

            if (string.IsNullOrWhiteSpace(expression))
                return expression;
            if (targets == null || targets.Count() < 1)
                return expression;
            if (resources == null || resources.Count() < 1)
                return expression;

            var result = new StringBuilder(expression);
            var enUsCulture = CultureInfo.GetCultureInfo("en-US");

            foreach (var resourceReference in targets)
            {
                if (resources.TryGetValue(targetCulture, out var resource))
                {
                    if (resource.StringTable.TryGetValue(resourceReference.ResourceKey, out var localizedValue))
                    {
                        result = result.Remove(resourceReference.FoundIndex, resourceReference.FoundLength);
                        result = result.Insert(resourceReference.FoundIndex, localizedValue);
                    }
                    else if (allowFallbackToEnUs && resources.TryGetValue(enUsCulture, out var enUsResource))
                    {
                        if (enUsResource.StringTable.TryGetValue(resourceReference.ResourceKey, out var enUsLocalizedValue))
                        {
                            result = result.Remove(resourceReference.FoundIndex, resourceReference.FoundLength);
                            result = result.Insert(resourceReference.FoundIndex, enUsLocalizedValue);
                        }
                    }
                }
            }

            return result.ToString();
        }

        internal AdmxResourceReference(int foundIndex, int foundLength, string resourceType, string resourceKey)
        {
            _foundIndex = foundIndex;
            _foundLength = foundLength;
            _resourceType = resourceType;
            _resourceKey = resourceKey;
        }

        private readonly int _foundIndex;
        private readonly int _foundLength;
        private readonly string _resourceType;
        private readonly string _resourceKey;

        /// <summary>
        /// Gets the index of the found resource reference.
        /// </summary>
        public int FoundIndex => _foundIndex;

        /// <summary>
        /// Gets the length of the found resource reference.
        /// </summary>
        public int FoundLength => _foundLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdmxResourceReference"/> class.
        /// </summary>
        public string ResourceType => _resourceType;

        /// <summary>
        /// Gets the resource key.
        /// </summary>
        public string ResourceKey => _resourceKey;

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
            => $"$({_resourceType}.{_resourceKey})";
    }
}
