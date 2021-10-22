using CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Tokens;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{
    public record RelationExpression(Token Token, Expression Expression1, Expression Expression2)
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
            Expression expression1 = Expression1.Reduce();
            Expression expression2 = Expression2.Reduce();
            string test = $"{expression1} {OperationToken} {expression2}";
            EmitJumps(test, t, f);
        }
    }
}
