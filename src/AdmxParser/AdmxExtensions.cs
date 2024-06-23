using AdmxParser.Serialization;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AdmxParser
{
    /// <summary>
    /// Represents a collection of extension methods for ADMX objects.
    /// </summary>
    public static class AdmxExtensions
    {
        /// <summary>
        /// Tries to get the local machine registry key.
        /// </summary>
        /// <param name="policy">
        /// The policy.
        /// </param>
        /// <param name="registryKey">
        /// The registry key.
        /// </param>
        /// <param name="writable">
        /// The writable.
        /// </param>
        /// <returns>
        /// If the local machine registry key was successfully retrieved, <c>true</c>; otherwise, <c>false</c>.
        /// </returns>
        public static bool TryGetLocalMachineRegistryKey(this PolicyDefinition policy, out RegistryKey registryKey, bool writable)
        {
            registryKey = default;

            switch (policy.@class)
            {
                case PolicyClass.Machine:
                case PolicyClass.Both:
                    try
                    {
                        registryKey = Registry.LocalMachine.OpenSubKey(policy.key, writable);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }

                default:
                    return false;
            }
        }

        /// <summary>
        /// Tries to get the current user registry key.
        /// </summary>
        /// <param name="policy">
        /// The policy.
        /// </param>
        /// <param name="registryKey">
        /// The registry key.
        /// </param>
        /// <param name="writable">
        /// The writable.
        /// </param>
        /// <returns>
        /// If the current user registry key was successfully retrieved, <c>true</c>; otherwise, <c>false</c>.
        /// </returns>
        public static bool TryGetCurrentUserRegistryKey(this PolicyDefinition policy, out RegistryKey registryKey, bool writable)
        {
            registryKey = default;

            switch (policy.@class)
            {
                case PolicyClass.User:
                case PolicyClass.Both:
                    try
                    {
                        registryKey = Registry.CurrentUser.OpenSubKey(policy.key, writable);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }

                default:
                    return false;
            }
        }

        /// <summary>
        /// Tries to get the user registry key by SID.
        /// </summary>
        /// <param name="policy">
        /// The policy.
        /// </param>
        /// <param name="userSid">
        /// The user SID.
        /// </param>
        /// <param name="registryKey">
        /// The registry key.
        /// </param>
        /// <param name="writable">
        /// The writable.
        /// </param>
        /// <returns>
        /// If the user registry key was successfully retrieved, <c>true</c>; otherwise, <c>false</c>.
        /// </returns>
        public static bool TryGetUserRegistryKeyBySid(this PolicyDefinition policy, string userSid, out RegistryKey registryKey, bool writable)
        {
            registryKey = default;

            switch (policy.@class)
            {
                case PolicyClass.User:
                case PolicyClass.Both:
                    try
                    {
                        registryKey = Registry.Users.OpenSubKey($"{userSid}\\{policy.key}", writable);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }

                default:
                    return false;
            }
        }

        /// <summary>
        /// Gets the localized display name.
        /// </summary>
        /// <param name="item">The localizable item.</param>
        /// <param name="resources">The resources.</param>
        /// <param name="targetCulture">The target culture.</param>
        /// <param name="allowFallbackToEnUs">Whether to allow fallback to en-US.</param>
        /// <returns>
        /// The localized display name.
        /// </returns>
        public static string GetLocalizedDisplayName(this EnumerationElementItem item, IReadOnlyDictionary<CultureInfo, AdmlResource> resources, CultureInfo targetCulture, bool allowFallbackToEnUs = true)
            => AdmxResourceReference.Interpolate(item.displayName, resources, targetCulture, allowFallbackToEnUs);

        /// <summary>
        /// Gets the localized display name.
        /// </summary>
        /// <param name="item">The localizable item.</param>
        /// <param name="resources">The resources.</param>
        /// <param name="targetCulture">The target culture.</param>
        /// <param name="allowFallbackToEnUs">Whether to allow fallback to en-US.</param>
        /// <returns>
        /// The localized display name.
        /// </returns>
        public static string GetLocalizedDisplayName(this PolicyDefinition item, IReadOnlyDictionary<CultureInfo, AdmlResource> resources, CultureInfo targetCulture, bool allowFallbackToEnUs = true)
            => AdmxResourceReference.Interpolate(item.displayName, resources, targetCulture, allowFallbackToEnUs);

        /// <summary>
        /// Gets the localized display name.
        /// </summary>
        /// <param name="item">The localizable item.</param>
        /// <param name="resources">The resources.</param>
        /// <param name="targetCulture">The target culture.</param>
        /// <param name="allowFallbackToEnUs">Whether to allow fallback to en-US.</param>
        /// <returns>
        /// The localized display name.
        /// </returns>
        public static string GetLocalizedDisplayName(this Category item, IReadOnlyDictionary<CultureInfo, AdmlResource> resources, CultureInfo targetCulture, bool allowFallbackToEnUs = true)
            => AdmxResourceReference.Interpolate(item.displayName, resources, targetCulture, allowFallbackToEnUs);

        /// <summary>
        /// Gets the localized display name.
        /// </summary>
        /// <param name="item">The localizable item.</param>
        /// <param name="resources">The resources.</param>
        /// <param name="targetCulture">The target culture.</param>
        /// <param name="allowFallbackToEnUs">Whether to allow fallback to en-US.</param>
        /// <returns>
        /// The localized display name.
        /// </returns>
        public static string GetLocalizedDisplayName(this SupportedOnDefinition item, IReadOnlyDictionary<CultureInfo, AdmlResource> resources, CultureInfo targetCulture, bool allowFallbackToEnUs = true)
            => AdmxResourceReference.Interpolate(item.displayName, resources, targetCulture, allowFallbackToEnUs);

        /// <summary>
        /// Gets the localized display name.
        /// </summary>
        /// <param name="item">The localizable item.</param>
        /// <param name="resources">The resources.</param>
        /// <param name="targetCulture">The target culture.</param>
        /// <param name="allowFallbackToEnUs">Whether to allow fallback to en-US.</param>
        /// <returns>
        /// The localized display name.
        /// </returns>
        public static string GetLocalizedDisplayName(this SupportedMinorVersion item, IReadOnlyDictionary<CultureInfo, AdmlResource> resources, CultureInfo targetCulture, bool allowFallbackToEnUs = true)
            => AdmxResourceReference.Interpolate(item.displayName, resources, targetCulture, allowFallbackToEnUs);

        /// <summary>
        /// Gets the localized display name.
        /// </summary>
        /// <param name="item">The localizable item.</param>
        /// <param name="resources">The resources.</param>
        /// <param name="targetCulture">The target culture.</param>
        /// <param name="allowFallbackToEnUs">Whether to allow fallback to en-US.</param>
        /// <returns>
        /// The localized display name.
        /// </returns>
        public static string GetLocalizedDisplayName(this SupportedMajorVersion item, IReadOnlyDictionary<CultureInfo, AdmlResource> resources, CultureInfo targetCulture, bool allowFallbackToEnUs = true)
            => AdmxResourceReference.Interpolate(item.displayName, resources, targetCulture, allowFallbackToEnUs);

        /// <summary>
        /// Gets the localized display name.
        /// </summary>
        /// <param name="item">The localizable item.</param>
        /// <param name="resources">The resources.</param>
        /// <param name="targetCulture">The target culture.</param>
        /// <param name="allowFallbackToEnUs">Whether to allow fallback to en-US.</param>
        /// <returns>
        /// The localized display name.
        /// </returns>
        public static string GetLocalizedDisplayName(this SupportedProduct item, IReadOnlyDictionary<CultureInfo, AdmlResource> resources, CultureInfo targetCulture, bool allowFallbackToEnUs = true)
            => AdmxResourceReference.Interpolate(item.displayName, resources, targetCulture, allowFallbackToEnUs);

        /// <summary>
        /// Gets the localized display name.
        /// </summary>
        /// <param name="item">The localizable item.</param>
        /// <param name="resources">The resources.</param>
        /// <param name="targetCulture">The target culture.</param>
        /// <param name="allowFallbackToEnUs">Whether to allow fallback to en-US.</param>
        /// <returns>
        /// The localized display name.
        /// </returns>
        public static string GetLocalizedDisplayName(this PolicyDefinitionResources item, IReadOnlyDictionary<CultureInfo, AdmlResource> resources, CultureInfo targetCulture, bool allowFallbackToEnUs = true)
            => AdmxResourceReference.Interpolate(item.displayName, resources, targetCulture, allowFallbackToEnUs);

        /// <summary>
        /// Gets the localized explain text.
        /// </summary>
        /// <param name="item">
        /// The explainable item.
        /// </param>
        /// <param name="resources">
        /// The resources.
        /// </param>
        /// <param name="targetCulture">
        /// The target culture.
        /// </param>
        /// <param name="allowFallbackToEnUs">
        /// Whether to allow fallback to en-US.
        /// </param>
        /// <returns></returns>
        public static string GetLocalizedExplainTextInternal(PolicyDefinition item,
            IReadOnlyDictionary<CultureInfo, AdmlResource> resources,
            CultureInfo targetCulture,
            bool allowFallbackToEnUs = true)
            => AdmxResourceReference.Interpolate(item.explainText, resources, targetCulture, allowFallbackToEnUs);

        /// <summary>
        /// Gets the localized explain text.
        /// </summary>
        /// <param name="item">
        /// The explainable item.
        /// </param>
        /// <param name="resources">
        /// The resources.
        /// </param>
        /// <param name="targetCulture">
        /// The target culture.
        /// </param>
        /// <param name="allowFallbackToEnUs">
        /// Whether to allow fallback to en-US.
        /// </param>
        /// <returns></returns>
        public static string GetLocalizedExplainTextInternal(Category item,
            IReadOnlyDictionary<CultureInfo, AdmlResource> resources,
            CultureInfo targetCulture,
            bool allowFallbackToEnUs = true)
            => AdmxResourceReference.Interpolate(item.explainText, resources, targetCulture, allowFallbackToEnUs);

        /// <summary>
        /// Gets the mangled member name from the display name.
        /// </summary>
        /// <param name="item">
        /// The localizable item.
        /// </param>
        /// <param name="separator">
        /// The separator.
        /// </param>
        /// <returns>
        /// The mangled member name.
        /// </returns>
        public static string GetMangledMemberNameFromDisplayName(this EnumerationElementItem item, string separator = default)
            => string.Join(separator ?? string.Empty, AdmxResourceReference.Parse(item.displayName).Select(x => x.ResourceKey));

        /// <summary>
        /// Gets the mangled member name from the display name.
        /// </summary>
        /// <param name="item">
        /// The localizable item.
        /// </param>
        /// <param name="separator">
        /// The separator.
        /// </param>
        /// <returns>
        /// The mangled member name.
        /// </returns>
        public static string GetMangledMemberNameFromDisplayName(this PolicyDefinition item, string separator = default)
            => string.Join(separator ?? string.Empty, AdmxResourceReference.Parse(item.displayName).Select(x => x.ResourceKey));

        /// <summary>
        /// Gets the mangled member name from the display name.
        /// </summary>
        /// <param name="item">
        /// The localizable item.
        /// </param>
        /// <param name="separator">
        /// The separator.
        /// </param>
        /// <returns>
        /// The mangled member name.
        /// </returns>
        public static string GetMangledMemberNameFromDisplayName(this Category item, string separator = default)
            => string.Join(separator ?? string.Empty, AdmxResourceReference.Parse(item.displayName).Select(x => x.ResourceKey));

        /// <summary>
        /// Gets the mangled member name from the display name.
        /// </summary>
        /// <param name="item">
        /// The localizable item.
        /// </param>
        /// <param name="separator">
        /// The separator.
        /// </param>
        /// <returns>
        /// The mangled member name.
        /// </returns>
        public static string GetMangledMemberNameFromDisplayName(this SupportedOnDefinition item, string separator = default)
            => string.Join(separator ?? string.Empty, AdmxResourceReference.Parse(item.displayName).Select(x => x.ResourceKey));

        /// <summary>
        /// Gets the mangled member name from the display name.
        /// </summary>
        /// <param name="item">
        /// The localizable item.
        /// </param>
        /// <param name="separator">
        /// The separator.
        /// </param>
        /// <returns>
        /// The mangled member name.
        /// </returns>
        public static string GetMangledMemberNameFromDisplayName(this SupportedMinorVersion item, string separator = default)
            => string.Join(separator ?? string.Empty, AdmxResourceReference.Parse(item.displayName).Select(x => x.ResourceKey));

        /// <summary>
        /// Gets the mangled member name from the display name.
        /// </summary>
        /// <param name="item">
        /// The localizable item.
        /// </param>
        /// <param name="separator">
        /// The separator.
        /// </param>
        /// <returns>
        /// The mangled member name.
        /// </returns>
        public static string GetMangledMemberNameFromDisplayName(this SupportedMajorVersion item, string separator = default)
            => string.Join(separator ?? string.Empty, AdmxResourceReference.Parse(item.displayName).Select(x => x.ResourceKey));

        /// <summary>
        /// Gets the mangled member name from the display name.
        /// </summary>
        /// <param name="item">
        /// The localizable item.
        /// </param>
        /// <param name="separator">
        /// The separator.
        /// </param>
        /// <returns>
        /// The mangled member name.
        /// </returns>
        public static string GetMangledMemberNameFromDisplayName(this SupportedProduct item, string separator = default)
            => string.Join(separator ?? string.Empty, AdmxResourceReference.Parse(item.displayName).Select(x => x.ResourceKey));

        /// <summary>
        /// Gets the mangled member name from the display name.
        /// </summary>
        /// <param name="item">
        /// The localizable item.
        /// </param>
        /// <param name="separator">
        /// The separator.
        /// </param>
        /// <returns>
        /// The mangled member name.
        /// </returns>
        public static string GetMangledMemberNameFromDisplayName(this PolicyDefinitionResources item, string separator = default)
            => string.Join(separator ?? string.Empty, AdmxResourceReference.Parse(item.displayName).Select(x => x.ResourceKey));

        /// <summary>
        /// Converts the specified integer value to an ADMX value.
        /// </summary>
        /// <param name="value">
        /// The integer value to convert.
        /// </param>
        /// <returns>
        /// The ADMX value.
        /// </returns>
        public static Value ToAdmxValue(this int value)
            => new Value() { Item = new ValueDecimal() { value = unchecked((uint)value), }, };

        /// <summary>
        /// Converts the specified nullable integer value to an ADMX value.
        /// </summary>
        /// <param name="value">
        /// The nullable integer value to convert.
        /// </param>
        /// <returns>
        /// The ADMX value.
        /// </returns>
        public static Value ToAdmxValue(this int? value)
            => value.HasValue ? new Value() { Item = new ValueDecimal() { value = unchecked((uint)value.Value), }, } : null;

        /// <summary>
        /// Converts the specified unsigned integer value to an ADMX value.
        /// </summary>
        /// <param name="value">
        /// The unsigned integer value to convert.
        /// </param>
        /// <returns>
        /// The ADMX value.
        /// </returns>
        public static Value ToAdmxValue(this uint value)
            => new Value() { Item = new ValueDecimal() { value = value, }, };

        /// <summary>
        /// Converts the specified nullable unsigned integer value to an ADMX value.
        /// </summary>
        /// <param name="value">
        /// The nullable unsigned integer value to convert.
        /// </param>
        /// <returns>
        /// The ADMX value.
        /// </returns>
        public static Value ToAdmxValue(this uint? value)
            => value.HasValue ? new Value() { Item = new ValueDecimal() { value = value.Value, }, } : null;

        /// <summary>
        /// Converts the specified long value to an ADMX value.
        /// </summary>
        /// <param name="value">
        /// The long value to convert.
        /// </param>
        /// <returns>
        /// The ADMX value.
        /// </returns>
        public static Value ToAdmxValue(this long value)
            => new Value() { Item = new ValueLongDecimal() { value = unchecked((ulong)value), }, };

        /// <summary>
        /// Converts the specified nullable long value to an ADMX value.
        /// </summary>
        /// <param name="value">
        /// The nullable long value to convert.
        /// </param>
        /// <returns>
        /// The ADMX value.
        /// </returns>
        public static Value ToAdmxValue(this long? value)
            => value.HasValue ? new Value() { Item = new ValueLongDecimal() { value = unchecked((ulong)value.Value), }, } : null;

        /// <summary>
        /// Converts the specified unsigned long value to an ADMX value.
        /// </summary>
        /// <param name="value">
        /// The unsigned long value to convert.
        /// </param>
        /// <returns>
        /// The ADMX value.
        /// </returns>
        public static Value ToAdmxValue(this ulong value)
            => new Value() { Item = new ValueLongDecimal() { value = value, }, };

        /// <summary>
        /// Converts the specified nullable unsigned long value to an ADMX value.
        /// </summary>
        /// <param name="value">
        /// The nullable unsigned long value to convert.
        /// </param>
        /// <returns>
        /// The ADMX value.
        /// </returns>
        public static Value ToAdmxValue(this ulong? value)
            => value.HasValue ? new Value() { Item = new ValueLongDecimal() { value = value.Value, }, } : null;

        /// <summary>
        /// Converts the specified string value to an ADMX value.
        /// </summary>
        /// <param name="value">
        /// The string value to convert.
        /// </param>
        /// <returns>
        /// The ADMX value.
        /// </returns>
        public static Value ToAdmxValue(this string value)
            => value != null ? new Value() { Item = value, } : null;
        
        /// <summary>
        /// Converts the specified ADMX value to an integer value.
        /// </summary>
        /// <param name="value">
        /// The ADMX value to convert.
        /// </param>
        /// <returns>
        /// The integer value.
        /// </returns>
        public static int? GetInt32(this Value value)
        {
            if (value == null)
                return null;
            if (value.Item is ValueDecimal)
                return unchecked((int)((ValueDecimal)value.Item).value);
            return null;
        }

        /// <summary>
        /// Converts the specified ADMX value to an unsigned integer value.
        /// </summary>
        /// <param name="value">
        /// The ADMX value to convert.
        /// </param>
        /// <returns>
        /// The unsigned integer value.
        /// </returns>
        public static uint? GetUInt32(this Value value)
        {
            if (value == null)
                return null;
            if (value.Item is ValueDecimal)
                return ((ValueDecimal)value.Item).value;
            return null;
        }

        /// <summary>
        /// Converts the specified ADMX value to a long value.
        /// </summary>
        /// <param name="value">
        /// The ADMX value to convert.
        /// </param>
        /// <returns>
        /// The long value.
        /// </returns>
        public static long? GetInt64(this Value value)
        {
            if (value == null)
                return null;
            if (value.Item is ValueLongDecimal)
                return unchecked((long)((ValueLongDecimal)value.Item).value);
            return null;
        }

        /// <summary>
        /// Converts the specified ADMX value to an unsigned long value.
        /// </summary>
        /// <param name="value">
        /// The ADMX value to convert.
        /// </param>
        /// <returns>
        /// The unsigned long value.
        /// </returns>
        public static ulong? GetUInt64(this Value value)
        {
            if (value == null)
                return null;
            if (value.Item is ValueLongDecimal)
                return ((ValueLongDecimal)value.Item).value;
            return null;
        }

        /// <summary>
        /// Converts the specified ADMX value to a string value.
        /// </summary>
        /// <param name="value">
        /// The ADMX value to convert.
        /// </param>
        /// <returns>
        /// The string value.
        /// </returns>
        public static string GetString(this Value value)
        {
            if (value == null)
                return null;
            if (value.Item is string)
                return (string)value.Item;
            return null;
        }

        /// <summary>
        /// Determines whether the specified ADMX value is a delete value.
        /// </summary>
        /// <param name="value">
        /// The ADMX value.
        /// </param>
        /// <returns>
        /// If the ADMX value is a delete value, <c>true</c>; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDeleteValue(this Value value)
            => value?.Item is ValueDelete;

        /// <summary>
        /// Converts the specified registry key to a registry hive.
        /// </summary>
        /// <param name="registryKey">
        /// The registry key.
        /// </param>
        /// <returns>
        /// The registry hive.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="registryKey"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the specified registry hive is not supported.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the specified registry hive name is invalid.
        /// </exception>
        public static RegistryHive GetRegistryHive(this RegistryKey registryKey)
        {
            if (registryKey == null)
                throw new ArgumentNullException(nameof(registryKey));

            var keyPath = registryKey.Name;
            var firstBackslashIndex = keyPath.IndexOf('\\');
            var hiveName = keyPath.Substring(0, firstBackslashIndex);
            var subKeyPath = keyPath.Substring(firstBackslashIndex + 1);

            switch (hiveName.ToUpperInvariant())
            {
                case "HKCR":
                case "HKEY_CLASSES_ROOT":
                    return RegistryHive.ClassesRoot;
                case "HKCU":
                case "HKEY_CURRENT_USER":
                    return RegistryHive.CurrentUser;
                case "HKLM":
                case "HKEY_LOCAL_MACHINE":
                    return RegistryHive.LocalMachine;
                case "HKU":
                case "HKEY_USERS":
                    return RegistryHive.Users;
                case "HKCC":
                case "HKEY_CURRENT_CONFIG":
                    return RegistryHive.CurrentConfig;
                case "HKPD":
                case "HKEY_PERFORMANCE_DATA":
                    return RegistryHive.PerformanceData;
                case "HKDD":
                case "HKEY_DYN_DATA":
                    throw new InvalidOperationException($"{hiveName} is not supported.");
                default:
                    throw new ArgumentException($"Invalid hive name: {hiveName}");
            }
        }

        /// <summary>
        /// Determines whether the specified registry key has a value.
        /// </summary>
        /// <remarks>
        /// In the Registry Editor, the so-called default entry is not always present, but rather an entry with a key name string of length 0 is added when the entry is set.
        /// </remarks>
        /// <param name="registryKey">
        /// The registry key.
        /// </param>
        /// <param name="name">
        /// The value name.
        /// </param>
        /// <returns>
        /// If the value exists, <c>true</c>; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValueExists(this RegistryKey registryKey, string name)
            => registryKey.GetValueNames().Contains(name ?? string.Empty, StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets the value of the specified registry key.
        /// </summary>
        /// <param name="registryKey">
        /// The registry key.
        /// </param>
        /// <param name="name">
        /// The value name.
        /// </param>
        /// <param name="treatAsText">
        /// Whether to treat the value as text.
        /// </param>
        /// <returns>
        /// The value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the specified registry value is not supported.
        /// </exception>
        public static Value GetAdmxValue(this RegistryKey registryKey, string name, bool treatAsText)
        {
            if (registryKey == null)
                return null;
            if (!registryKey.IsValueExists(name))
                return null;
            var value = registryKey.GetValue(name);
            var valueKind = registryKey.GetValueKind(name);

            switch (valueKind)
            {
                case RegistryValueKind.DWord:
                    if (!treatAsText)
                    {
                        if (value is int)
                            return new Value() { Item = new ValueDecimal() { value = unchecked((uint)(int)value), }, };
                        if (value is uint)
                            return new Value() { Item = new ValueDecimal() { value = (uint)value, }, };
                    }
                    else
                        return new Value() { Item = Convert.ToString(value), };
                    break;
                case RegistryValueKind.QWord:
                    if (!treatAsText)
                    {
                        if (value is long)
                            return new Value() { Item = new ValueLongDecimal() { value = unchecked((ulong)(long)value), }, };
                        if (value is ulong)
                            return new Value() { Item = new ValueLongDecimal() { value = (ulong)value, }, };
                    }
                    else
                        return new Value() { Item = Convert.ToString(value), };
                    break;
                case RegistryValueKind.String:
                case RegistryValueKind.MultiString:
                case RegistryValueKind.ExpandString:
                    if (value is string)
                        return new Value() { Item = (string)value, };
                    break;
            }

            throw new ArgumentException($"Selected registry value '{name}' is not supported.", nameof(name));
        }

        /// <summary>
        /// Sets the value of the specified registry key.
        /// </summary>
        /// <param name="registryKey">
        /// The registry key.
        /// </param>
        /// <param name="name">
        /// The value name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="treatAsText">
        /// Whether to treat the value as text.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when the specified ADMX value is invalid.
        /// </exception>
        public static void SetAdmxValue(this RegistryKey registryKey, string name, Value value, bool treatAsText)
        {
            if (registryKey == null)
                return;
            if (value == null)
                return;
            name = name ?? string.Empty;
            if (value.Item is ValueDecimal)
            {
                var innerValue = unchecked((int)((ValueDecimal)value.Item).value);
                if (!treatAsText)
                    registryKey.SetValue(name, innerValue);
                else
                    registryKey.SetValue(name, Convert.ToString(innerValue));
            }
            else if (value.Item is ValueLongDecimal)
            {
                var innerValue = unchecked((long)((ValueDecimal)value.Item).value);
                if (!treatAsText)
                    registryKey.SetValue(name, innerValue);
                else
                    registryKey.SetValue(name, Convert.ToString(innerValue));
            }
            else if (value.Item is string)
                registryKey.SetValue(name, (string)value.Item);
            else if (value.Item is ValueDelete)
                registryKey.DeleteValue(name, false);
            else
                throw new ArgumentException("Unexpected ADMX value specified.", nameof(value));
        }

        /// <summary>
        /// Check equality of two ADMX values.
        /// </summary>
        /// <param name="leftValue">
        /// The left ADMX value.
        /// </param>
        /// <param name="rightValue">
        /// The right ADMX value.
        /// </param>
        /// <returns>
        /// If the ADMX values are equal, <c>true</c>; otherwise, <c>false</c>.
        /// </returns>
        public static bool Equals(this Value leftValue, Value rightValue)
        {
            if (leftValue == null)
                return (rightValue == null);
            if (leftValue.Item is ValueDelete && rightValue.Item is ValueDelete)
                return true;

            if (leftValue.Item is ValueDecimal)
            {
                var leftVD = (ValueDecimal)leftValue.Item;

                if (rightValue.Item is ValueDecimal)
                {
                    var rightVD = (ValueDecimal)rightValue.Item;
                    return leftVD.value == rightVD.value;
                }
            }

            if (leftValue.Item is ValueLongDecimal)
            {
                var leftVLD = (ValueLongDecimal)leftValue.Item;

                if (rightValue.Item is ValueLongDecimal)
                {
                    var rightVLD = (ValueLongDecimal)rightValue.Item;
                    return leftVLD.value == rightVLD.value;
                }
            }

            if (leftValue.Item is string)
            {
                var leftS = (string)leftValue.Item;

                if (rightValue.Item is string)
                {
                    var rightS = (string)rightValue.Item;
                    return string.Equals(leftS, rightS, StringComparison.Ordinal);
                }
            }

            return false;
        }

        /// <summary>
        /// Get string expression of ADMX value.
        /// </summary>
        /// <param name="value">
        /// The ADMX value.
        /// </param>
        /// <param name="attachPostfixToDecimal">
        /// Whether to attach postfix.
        /// </param>
        /// <returns>
        /// The string expression of the ADMX value.
        /// </returns>
        public static string GetStringExpression(this Value value, bool attachPostfixToDecimal = false)
        {
            if (value == null)
                return null;
            if (value.Item is ValueDecimal)
                return ((ValueDecimal)value.Item).value.ToString() + (attachPostfixToDecimal ? "u" : string.Empty);
            if (value.Item is ValueLongDecimal)
                return ((ValueLongDecimal)value.Item).value.ToString() + (attachPostfixToDecimal ? "uL" : string.Empty);
            if (value.Item is string)
                return (string)value.Item;
            if (value.Item is ValueDelete)
                return "<delete>";
            return null;
        }
    }
}
