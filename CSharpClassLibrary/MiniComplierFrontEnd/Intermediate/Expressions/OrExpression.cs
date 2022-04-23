using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;

public record OrExpression(Token Token, IExpression Expression1, IExpression Expression2, INode Node)
    : LogicalExpression(Token, Expression1, Expression2, Node)
{
    public override void Jumping(int t, int f)
    {
        int label = t != 0 ? t : Node.NewLabel();
        Expression1.Jumping(label, 0);
        Expression2.Jumping(t, f);
        if (t == 0) Node.EmitLabel(label);
    }
}
