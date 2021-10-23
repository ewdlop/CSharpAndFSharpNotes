namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{
    public interface IExpression : IExpressionEmitable, IReadonlyExpression
    {
        IExpression Generate();
        IExpression Reduce();
        void Jumping(int t, int f);
    }
}
