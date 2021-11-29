using System.Runtime.InteropServices;

namespace CSharpClassLibrary.Native
{
    public static class NativPlatform
    {
        static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        // Cross platform C function
        // long Function(long a);

        [DllImport("NativeLib", EntryPoint = "Function")]
        extern static int FunctionWindows(int a);

        [DllImport("NativeLib", EntryPoint = "Function")]
        extern static nint FunctionUnix(nint a);

        public static void FunctionWindows()
        {
            // Usage
            nint result;
            if (IsWindows)
            {
                result = FunctionWindows(10);
            }
            else
            {
                result = FunctionUnix(10);
            }
        }
    }
}
