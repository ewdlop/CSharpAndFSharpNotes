using System;
using System.Runtime.InteropServices;

namespace CSharpClassLibrary.Native
{
    public static class CallBack
    {
        public static void CallbackMethod(int i)
        {
            Console.WriteLine(i);
        }

        private static Action<int> _callback = (x)=> CallbackMethod(x);
        private delegate void CallbackDelegate(int i);
        private static CallbackDelegate s_callback = new CallbackDelegate(CallbackMethod);

        [DllImport("NativeLib")]
        private static extern void NativeFunctionWithCallback(IntPtr callback);

        static void Main()
        {
            IntPtr callback = Marshal.GetFunctionPointerForDelegate(s_callback);
            IntPtr callback2 = Marshal.GetFunctionPointerForDelegate(_callback);
            NativeFunctionWithCallback(callback);
        }
    }

}
