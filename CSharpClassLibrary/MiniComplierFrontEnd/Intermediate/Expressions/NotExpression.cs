using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Emitters;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;

public record NotExpression(Token Token, IExpression Expression, ILabelEmitter Node)
    : LogicalExpression(Token, Expression, Expression, Node)
{
    public override void Jumping(int t, int f)
    {
        Expression.Jumping(f,t);
    }
    public override string ToString() => $"{OperationToken} {Expression}";
}
