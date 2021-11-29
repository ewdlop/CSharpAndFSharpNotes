namespace CSharpClassLibrary.CSharp9.FunctionPointer
{
    public unsafe struct Test
    {
        delegate int Func1(string s);
        delegate Func1 Func2(Func1 f);

        // Function pointer equivalent without calling convention
        delegate*<string, int> test;
        delegate*<delegate*<string, int>, delegate*<string, int>> test2;

        // Function pointer equivalent with calling convention
        delegate* managed<string, int> test3;
        delegate*<delegate* managed<string, int>, delegate*<string, int>> test4;
    }
}
