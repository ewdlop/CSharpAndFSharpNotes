using System;
using System.Runtime.InteropServices;

namespace CSharpClassLibrary.Native
{
    public static class NativeLogger
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate void CSCALLBACK(string msg);

        internal class NativeMethods
        {
            [DllImport("Logger.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            public static extern void RunLog(string str);

            [DllImport("Logger.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern void SetLogger(CSCALLBACK csFunc);

        }

        static void Logger(string msg)
        {
            Console.WriteLine(msg);
        }

        static void Test()
        {
            NativeMethods.SetLogger(Logger);

            NativeMethods.RunLog("First call");

            NativeMethods.RunLog("Second call");

            NativeMethods.RunLog("Last call");
        }
    }

}
