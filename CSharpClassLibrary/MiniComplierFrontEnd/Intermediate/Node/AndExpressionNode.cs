using CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Token;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{
    public record AndExpressionNode(Token Token, ExpressionNode ExpressionNode1, ExpressionNode ExpressionNode2)
        : LogicalExpressionNode(Token, ExpressionNode1, ExpressionNode2)
    {
        public override void Jumping(int t, int f)
        {
            int label = f != 0 ? t : NewLabel();
            ExpressionNode1.Jumping(0,label);
            ExpressionNode2.Jumping(t, f);
            if (t == 0) EmitLabel(label);
        }
    }
}
