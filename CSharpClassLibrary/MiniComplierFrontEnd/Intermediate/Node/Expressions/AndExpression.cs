using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{
    public record AndExpression(Token Token, IExpression Expression1, IExpression Expression2)
        : LogicalExpression(Token, Expression1, Expression2)
    {
        public override void Jumping(int t, int f)
        {
            int label = f != 0 ? t : NewLabel();
            Expression1.Jumping(0,label);
            Expression2.Jumping(t, f);
            if (t == 0) EmitLabel(label);
        }
    }
}
