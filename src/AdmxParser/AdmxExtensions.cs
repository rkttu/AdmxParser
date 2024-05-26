using AdmxParser.Models;
using Microsoft.Win32;

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
    }
}
