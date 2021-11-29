using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CSharpClassLibrary.Native
{
    public unsafe static class FunctionPointerWrapper
    {
        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvCdecl) })]
        public static int Callback(int i)
        {
            return ++i;
        }

        [DllImport("NativeLib")]
        private static extern void NativeFunctionWithCallback(delegate* unmanaged[Cdecl]<int, int> callback);

        static void Main()
        {
            delegate* unmanaged[Cdecl]<int, int> unmanagedPtr = &Callback;
            NativeFunctionWithCallback(unmanagedPtr);
        }
    }

}
