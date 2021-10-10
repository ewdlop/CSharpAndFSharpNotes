using CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Token;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{
    public record UnaryOperationExpressionNode(Token Token, ExpressionNode ExpressionNode)
        : OperationExpressionNode(Token, Symbols.TypeToken.Max(Symbols.TypeToken.INT, ExpressionNode.TypeToken))
    {
        public override ExpressionNode Generate() => new UnaryOperationExpressionNode(OperationToken, ExpressionNode.Reduce());
        public override string ToString()
        {
            return $"{OperationToken} {ExpressionNode}";
        }
    }
}
