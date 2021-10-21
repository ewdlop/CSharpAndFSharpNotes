using CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Token;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{
    public record NotExpressionNode(Token Token, ExpressionNode ExpressionNode2)
        : LogicalExpressionNode(Token, ExpressionNode2, ExpressionNode2)
    {
        public override void Jumping(int t, int f)
        {
            ExpressionNode2.Jumping(f,t);
        }
        public override string ToString() => $"{OperationToken} {ExpressionNode2}";
    }
}
