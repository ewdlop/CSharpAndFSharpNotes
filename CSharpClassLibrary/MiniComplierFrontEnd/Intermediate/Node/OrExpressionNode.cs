using CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Token;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{
    public record OrExpressionNode(Token Token, ExpressionNode ExpressionNode1, ExpressionNode ExpressionNode2)
        : LogicalExpressionNode(Token, ExpressionNode1, ExpressionNode2)
    {
        public override void Jumping(int t, int f)
        {
            int label = t != 0 ? t : NewLabel();
            ExpressionNode1.Jumping(label, 0);
            ExpressionNode2.Jumping(t, f);
            if (t == 0) EmitLabel(label);
        }
    }
}
