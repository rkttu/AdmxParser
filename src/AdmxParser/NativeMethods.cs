using System.Runtime.InteropServices;

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
    }

}
