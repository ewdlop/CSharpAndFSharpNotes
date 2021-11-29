using System;
using System.Runtime.InteropServices;

namespace CSharpClassLibrary.Native
{
    static class HSTRING
    {
        public static IntPtr FromString(string s)
        {
            Marshal.ThrowExceptionForHR(WindowsCreateString(s, s.Length, out IntPtr h));
            return h;
        }

        public static void Delete(IntPtr s)
        {
            Marshal.ThrowExceptionForHR(WindowsDeleteString(s));
        }

        [DllImport("api-ms-win-core-winrt-string-l1-1-0.dll", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        private static extern int WindowsCreateString(
            [MarshalAs(UnmanagedType.LPWStr)] string sourceString, int length, out IntPtr hstring);

        [DllImport("api-ms-win-core-winrt-string-l1-1-0.dll", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        private static extern int WindowsDeleteString(IntPtr hstring);
    }
}
