using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{
    public record ArithmeticOperationExpression(Token Token, IExpression Expression1, IExpression Expression2)
        : OperationExpression(Token, Symbols.TypeToken.Max(Expression1.TypeToken, Expression2.TypeToken))
    {
        public override Expression Generate() => new ArithmeticOperationExpression(OperationToken, Expression1.Reduce(), Expression2.Reduce());
        public override string ToString() => $"{Expression1} {OperationToken} {Expression2}";
    }
}