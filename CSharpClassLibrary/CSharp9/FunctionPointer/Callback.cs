

using System;

namespace CSharpClassLibrary.CSharp9.FunctionPointer
{
    unsafe class CallBack
    {
        public static void Log() { }
        public static void Log(string p1) { }
        public static void Log(int i) { }

        delegate int Func1(string s);
        private delegate Func1 Func2(Func1 f);


        void Use()
        {
            delegate*<void> p1 = &Log;
            delegate*<int, void> p2 = &Log;
            delegate* managed<void> p3 = p1;
            Console.WriteLine(p1 == p3); // True
        }
    }
}
