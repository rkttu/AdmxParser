using System.ComponentModel;
using System.Runtime.InteropServices;

namespace AdmxParser
{
    /// <summary>
    /// Contains methods for updating group policy.
    /// </summary>
    public static class UpdatePolicy
    {
        /// <summary>
        /// Perform a machine-wide policy update.
        /// </summary>
        /// <param name="forceRefresh">
        /// Whether to force a refresh.
        /// </param>
        /// <param name="policyRefreshFailureReason">
        /// The reason for the policy refresh failure.
        /// </param>
        /// <returns>
        /// Whether the policy update was successful.
        /// </returns>
        public static bool PerformMachinePolicyUpdate(bool forceRefresh, out Win32Exception policyRefreshFailureReason)
        {
            policyRefreshFailureReason = default;
            var result = false;

            if (forceRefresh)
                result = NativeMethods.RefreshPolicyEx(true, NativeMethods.RP_FORCE);
            else
                result = NativeMethods.RefreshPolicy(true);

            if (!result)
                policyRefreshFailureReason = new Win32Exception(Marshal.GetLastWin32Error());

            return result;
        }

        /// <summary>
        /// Perform a current user-wide policy update.
        /// </summary>
        /// <param name="forceRefresh">
        /// Whether to force a refresh.
        /// </param>
        /// <param name="policyRefreshFailureReason">
        /// The reason for the policy refresh failure.
        /// </param>
        /// <returns>
        /// Whether the policy update was successful.
        /// </returns>
        public static bool PerformCurrentUserPolicyUpdate(bool forceRefresh, out Win32Exception policyRefreshFailureReason)
        {
            policyRefreshFailureReason = default;
            var result = false;

            if (forceRefresh)
                result = NativeMethods.RefreshPolicyEx(false, NativeMethods.RP_FORCE);
            else
                result = NativeMethods.RefreshPolicy(false);

            if (!result)
                policyRefreshFailureReason = new Win32Exception(Marshal.GetLastWin32Error());

            return result;
        }

        /// <summary>
        /// Perform a full policy update (both local machine and current user).
        /// </summary>
        /// <param name="forceRefresh">
        /// Whether to force a refresh.
        /// </param>
        /// <param name="machinePolicyRefreshFailureReason">
        /// The reason for the machine policy refresh failure.
        /// </param>
        /// <param name="currentUserPolicyRefreshFailureReason">
        /// The reason for the current user policy refresh failure.
        /// </param>
        /// <returns>
        /// Whether the policy update was successful.
        /// </returns>
        public static bool PerformFullPolicyUpdate(bool forceRefresh, out Win32Exception machinePolicyRefreshFailureReason, out Win32Exception currentUserPolicyRefreshFailureReason)
        {
            machinePolicyRefreshFailureReason = default;
            currentUserPolicyRefreshFailureReason = default;

            if (forceRefresh)
            {
                var forceMachineRefresh = NativeMethods.RefreshPolicyEx(true, NativeMethods.RP_FORCE);

                if (!forceMachineRefresh)
                    machinePolicyRefreshFailureReason = new Win32Exception(Marshal.GetLastWin32Error());

                var forceCurrentUserRefresh = NativeMethods.RefreshPolicyEx(false, NativeMethods.RP_FORCE);

                if (!forceCurrentUserRefresh)
                    currentUserPolicyRefreshFailureReason = new Win32Exception(Marshal.GetLastWin32Error());

                return forceMachineRefresh & forceCurrentUserRefresh;
            }
            else
            {
                var machineRefresh = NativeMethods.RefreshPolicy(true);

                if (!machineRefresh)
                    machinePolicyRefreshFailureReason = new Win32Exception(Marshal.GetLastWin32Error());

                var currentUserRefresh = NativeMethods.RefreshPolicy(false);

                if (!currentUserRefresh)
                    currentUserPolicyRefreshFailureReason = new Win32Exception(Marshal.GetLastWin32Error());

                return machineRefresh & currentUserRefresh;
            }
        }
    }
}
