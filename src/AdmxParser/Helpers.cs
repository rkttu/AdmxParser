using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AdmxParser
{
    internal static class Helpers
    {
        static Helpers()
        {
            _availableCultures = new Lazy<IEnumerable<CultureInfo>>(
                () => CultureInfo.GetCultures(CultureTypes.AllCultures),
                LazyThreadSafetyMode.None);

            _policyDefinitionNamespaces = new Lazy<IEnumerable<string>>(
                () => new string[]
                {
                    "http://schemas.microsoft.com/GroupPolicy/2006/07/PolicyDefinitions",
                    "http://www.microsoft.com/GroupPolicy/PolicyDefinitions",
                    "http://schemas.microsoft.com/GroupPolicy/2006/07/Policysecurity intelligence",
                    "https://schemas.microsoft.com/GroupPolicy/2006/07/PolicyDefinitions",
                },
                LazyThreadSafetyMode.None);

            _parentCategoryRefSeparators = new Lazy<char[]>(
                () => new char[] { ':', },
                LazyThreadSafetyMode.None);
        }

        private static readonly Lazy<IEnumerable<CultureInfo>> _availableCultures;
        private static readonly Lazy<IEnumerable<string>> _policyDefinitionNamespaces;
        private static readonly Lazy<char[]> _parentCategoryRefSeparators;

        public static IEnumerable<CultureInfo> AvailableCultures => _availableCultures.Value;

        public static XmlNamespaceManager DiscoverDefaultNamespacePrefix(XDocument document, string expectedRootElementName, out string foundPathPrefix, out string foundNamespaceUri, XmlNameTable nameTable = default)
        {
            foundPathPrefix = string.Empty;
            foundNamespaceUri = string.Empty;

            var pathPrefix = "x:";
            var rootElement = default(XElement);
            var existingNamespace = default(string);
            var nsManager = new XmlNamespaceManager(nameTable ?? new NameTable());

            var nsCandidates = _policyDefinitionNamespaces.Value;
            foreach (var eachCandidate in nsCandidates)
            {
                existingNamespace = nsManager.LookupNamespace("x");
                if (existingNamespace != default)
                    nsManager.RemoveNamespace("x", existingNamespace);
                nsManager.AddNamespace("x", eachCandidate);

                rootElement = document.XPathSelectElement($"/{pathPrefix}{expectedRootElementName}", nsManager);
                if (rootElement == default)
                    continue;

                foundPathPrefix = pathPrefix;
                foundNamespaceUri = eachCandidate;
                return nsManager;
            }

            existingNamespace = nsManager.LookupNamespace("x");
            if (existingNamespace != default)
                nsManager.RemoveNamespace("x", existingNamespace);
            pathPrefix = string.Empty;

            foundPathPrefix = string.Empty;
            foundNamespaceUri = string.Empty;
            return nsManager;
        }

        public static IEnumerable<string> SafeEnumerateDirectories(string directoryPath, string searchPattern, SearchOption searchOption,
            Action<Exception> exceptionCatcher = default)
        {
            var source = Directory.EnumerateDirectories(directoryPath, searchPattern, searchOption);
            var enumerator = source.GetEnumerator();

            while (true)
            {
                try
                {
                    if (!enumerator.MoveNext())
                        break;
                }
                catch (Exception ex)
                {
                    exceptionCatcher?.Invoke(ex);
                    continue;
                }

                yield return enumerator.Current;
            }
        }

        public static IEnumerable<string> SafeEnumerateFiles(string directoryPath, string searchPattern, SearchOption searchOption,
            Action<Exception> exceptionCatcher = default)
        {
            var source = Directory.EnumerateFiles(directoryPath, searchPattern, searchOption);
            var enumerator = source.GetEnumerator();

            while (true)
            {
                try
                {
                    if (!enumerator.MoveNext())
                        break;
                }
                catch (Exception ex)
                {
                    exceptionCatcher?.Invoke(ex);
                    continue;
                }

                yield return enumerator.Current;
            }
        }

        public static bool TryParseResourceExpression(string expression, out string resourceType, out string resourceKey)
        {
            resourceType = default;
            resourceKey = default;

            if (string.IsNullOrWhiteSpace(expression))
                return false;

            var pattern = @"^\$\((?<resourceType>[^.]+)\.(?<resourceKey>[^)]+)\)$";
            var match = Regex.Match(expression, pattern, RegexOptions.Compiled);

            if (!match.Success)
                return false;

            resourceType = match.Groups["resourceType"].Value;
            resourceKey = match.Groups["resourceKey"].Value;
            return true;
        }

        public static bool TryParseCategoryRefExpression(string expression, out string targetNamespacePrefix, out string parentCategoryName)
        {
            targetNamespacePrefix = default;
            parentCategoryName = default;

            if (string.IsNullOrWhiteSpace(expression))
                return false;

            var separators = _parentCategoryRefSeparators.Value;
            var parts = expression.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length < 2)
                return false;

            targetNamespacePrefix = parts[0];
            parentCategoryName = parts[1];
            return true;
        }

        public static void EnsureDirectoryExists(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
                throw new ArgumentException("Directory path is required.", nameof(directoryPath));

            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Selected directory '{directoryPath}' does not exists on this system.");
        }

        public static void EnsureFileExists(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path is required.", nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Selected file '{filePath}' does not exists on this system.");
        }
    }
}
