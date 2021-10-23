using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{
    public record NotExpression(Token Token, IExpression Expression)
        : LogicalExpression(Token, Expression, Expression)
    {
        public override void Jumping(int t, int f)
        {
            Expression.Jumping(f,t);
        }
        public override string ToString() => $"{OperationToken} {Expression}";
    }
}
