namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions
{
    public interface IExpressionEmitable
    {
        void EmitJumps(string test, int t, int f);
    }
}
