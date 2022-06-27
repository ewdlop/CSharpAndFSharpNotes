using LLVMApp.Contexts;

Run();
unsafe static void Run()
{

    sbyte* name = stackalloc sbyte[] { Convert.ToSByte('J'), Convert.ToSByte('I'), Convert.ToSByte('T') };
    using LLVMContext context = new(name);
    context.Start();
}
