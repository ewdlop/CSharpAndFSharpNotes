using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Emitters;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;

public record AndExpression(Token Token, IExpression Expression1, IExpression Expression2, ILabelEmitter Node)
    : LogicalExpression(Token, Expression1, Expression2, Node)
{
    public override void Jumping(int t, int f)
    {
        int label = f != 0 ? t : Node.NewLabel();
        Expression1.Jumping(0,label);
        Expression2.Jumping(t, f);
        if (t == 0) Node.EmitLabel(label);
    }
    public override string ToString() => base.ToString();
}
