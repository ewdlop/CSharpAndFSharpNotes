namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions
{
    public interface IExpression : IExpressionEmitable, IReadOnlyExpression
    {
        IExpression Generate();
        IExpression Reduce();
        void Error(string message);
        void Jumping(int t, int f);
    }
}
