using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{
    public record UnaryOperation(Token Token, IExpression ExpressionNode)
        : OperationExpression(Token, Symbols.TypeToken.Max(Symbols.TypeToken.INT, ExpressionNode.TypeToken))
    {
        public override Expression Generate() => new UnaryOperation(OperationToken, ExpressionNode.Reduce());
        public override string ToString() => $"{OperationToken} {ExpressionNode}";
    }
}
