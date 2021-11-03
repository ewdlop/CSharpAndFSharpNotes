using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions
{
    public record UnaryOperationExpression(Token Token, IExpression ExpressionNode, INode Node)
        : OperationExpression(Token, Symbols.TypeToken.Max(Symbols.TypeToken.INT, ExpressionNode.TypeToken), Node)
    {
        public override Expression Generate() => new UnaryOperationExpression(OperationToken, ExpressionNode.Reduce(), Node);
        public override string ToString() => $"{OperationToken} {ExpressionNode}";
    }
}
