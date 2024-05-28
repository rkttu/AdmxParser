using System;
using System.Runtime.InteropServices;
using System.Text;

namespace AdmxParser
{
    internal static class NativeMethods
    {
        [DllImport("userenv.dll", SetLastError = true, CharSet = CharSet.None, CallingConvention = CallingConvention.Winapi, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RefreshPolicy(
            [MarshalAs(UnmanagedType.Bool)] bool bMachine);

        [DllImport("userenv.dll", SetLastError = true, CharSet = CharSet.None, CallingConvention = CallingConvention.Winapi, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RefreshPolicyEx(
            [MarshalAs(UnmanagedType.Bool)] bool bMachine,
            [MarshalAs(UnmanagedType.U4)] int dwOption);

        public static readonly int RP_FORCE = 1;

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
        public static extern bool LookupAccountNameW(
            string lpSystemName,
            string lpAccountName,
            byte[] Sid,
            [MarshalAs(UnmanagedType.U4)] ref int cbSid,
            StringBuilder ReferencedDomainName,
            [MarshalAs(UnmanagedType.U4)] ref int cchReferencedDomainName,
            out SidNameUse peUse);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.None, ExactSpelling = true)]
        public static extern bool ConvertSidToStringSidW(IntPtr Sid, out IntPtr StringSid);

        public enum SidNameUse : int
        {
            SidTypeUser = 1,
            SidTypeGroup,
            SidTypeDomain,
            SidTypeAlias,
            SidTypeWellKnownGroup,
            SidTypeDeletedAccount,
            SidTypeInvalid,
            SidTypeUnknown,
            SidTypeComputer
        }

        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.None)]
        public extern static IntPtr LocalFree(IntPtr hMem);
    }
}
