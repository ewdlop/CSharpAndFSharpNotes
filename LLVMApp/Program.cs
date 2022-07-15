using LLVMApp.Contexts;

namespace LLVMApp;

public sealed class Program
{
    public delegate double Delegate();

    public unsafe static void Main(string[] args)
    {
        sbyte* name = stackalloc sbyte[] { Convert.ToSByte('J'), Convert.ToSByte('I'), Convert.ToSByte('T') };
        using LLVMContext context = new(name);
        context.Start();
    }
}