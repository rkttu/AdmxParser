using AdmxParser.Models;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace AdmxParser
{
    /// <summary>
    /// Contains extension methods for the ADMX parser and models.
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
        public static bool TryGetLocalMachineRegistryKey(this Policy policy, out RegistryKey registryKey, bool writable)
        {
            registryKey = default;

            switch (policy.Class?.ToUpperInvariant())
            {
                case "MACHINE":
                case "BOTH":
                    try
                    {
                        registryKey = Registry.LocalMachine.OpenSubKey(policy.Key, writable);
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
        public static bool TryGetCurrentUserRegistryKey(this Policy policy, out RegistryKey registryKey, bool writable)
        {
            registryKey = default;

            switch (policy.Class?.ToUpperInvariant())
            {
                case "USER":
                case "BOTH":
                    try
                    {
                        registryKey = Registry.CurrentUser.OpenSubKey(policy.Key, writable);
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
        public static bool TryGetUserRegistryKeyBySid(this Policy policy, string userSid, out RegistryKey registryKey, bool writable)
        {
            registryKey = default;

            switch (policy.Class?.ToUpperInvariant())
            {
                case "USER":
                case "BOTH":
                    try
                    {
                        registryKey = Registry.Users.OpenSubKey($"{userSid}\\{policy.Key}", writable);
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
        /// Tries to get the user registry key by account name.
        /// </summary>
        /// <param name="policy">
        /// The policy.
        /// </param>
        /// <param name="accountName">
        /// The account name.
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
        public static bool TryGetUserRegistryByName(this Policy policy, string accountName, out RegistryKey registryKey, bool writable)
            => TryGetUserRegistryKeyBySid(policy, GetSidFromAccountName(accountName), out registryKey, writable);

        /// <summary>
        /// Gets the SID from an account name.
        /// </summary>
        /// <param name="accountName">
        /// The account name.
        /// </param>
        /// <returns>
        /// The SID of account.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="accountName"/> is <c>null</c>, empty, or whitespace.
        /// </exception>
        /// <exception cref="Win32Exception">
        /// An error occurred while trying to get the SID from the account name.
        /// </exception>
        public static string GetSidFromAccountName(string accountName)
        {
            if (string.IsNullOrWhiteSpace(accountName))
                throw new ArgumentNullException(nameof(accountName));
            
            var sidLength = 0;
            var domainNameLength = 0;
            var sidUse = default(NativeMethods.SidNameUse);
            var domainName = new StringBuilder();

            // First call to LookupAccountName to get the buffer sizes.
            NativeMethods.LookupAccountNameW(null, accountName, null, ref sidLength, null, ref domainNameLength, out sidUse);

            var sid = new byte[sidLength];
            domainName = new StringBuilder(domainNameLength);

            // Second call to LookupAccountName to get the actual SID.
            if (!NativeMethods.LookupAccountNameW(null, accountName, sid, ref sidLength, domainName, ref domainNameLength, out sidUse))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            // Convert the SID to a string.
            return Helpers.ConvertSidToStringSid(sid);
        }
    }
}
