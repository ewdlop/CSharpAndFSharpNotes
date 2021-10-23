using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{
    public record RelationExpression(Token Token, IExpression Expression1, IExpression Expression2)
        : LogicalExpression(Token, Expression1, Expression2)
    {
        new public static TypeToken Check(TypeToken typeToken1, TypeToken typeToken2)
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
    }
}
