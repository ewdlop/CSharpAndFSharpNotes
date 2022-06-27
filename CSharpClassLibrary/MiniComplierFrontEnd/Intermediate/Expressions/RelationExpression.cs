using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Emitters;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;

public record RelationExpression(Token Token, IExpression Expression1, IExpression Expression2, ILabelEmitter Node)
    : LogicalExpression(Token, Expression1, Expression2, Node, Check(Expression1.TypeToken, Expression2.TypeToken))
{
    public new static TypeToken? Check(TypeToken typeToken1, TypeToken typeToken2)
    {
        if(typeToken1 is ArrayTypeToken || typeToken2 is ArrayTypeToken)
        {
            return null;
        }
        else if(typeToken1 == typeToken2)
        {
            return TypeToken.BOOL;
        }
        else
        {
            return null;
        }
    }
    public override void Jumping(int t, int f)
    {
        string test = $"{Expression1.Reduce()} {OperationToken} {Expression2.Reduce()}";
        EmitJumps(test, t, f);
    }
    public override string ToString() => base.ToString();
}