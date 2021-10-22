using CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Tokens;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{
    public record LogicalExpression(Token Token, Expression Expression1, Expression Expression2)
        : Expression(Token, Check(Expression1.TypeToken, Expression2.TypeToken))
    {
        public static TypeToken Check(TypeToken typeToken1, TypeToken typeToken2)
        {
            if (typeToken1 == TypeToken.BOOL && typeToken2 == TypeToken.BOOL)
            {
                return TypeToken.BOOL;
            }
            else
            {
                throw new System.Exception("type error");
            }
        }

        public override Expression Generate()
        {
            int f = NewLabel();
            int a = NewLabel();
            var temporaryExpressionNode = new TemporaryExpression(TypeToken);
            Jumping(0, f);
            Emit($"{temporaryExpressionNode} = true");
            Emit($"goto L{a}");
            EmitLabel(f);
            Emit($"{temporaryExpressionNode} = false");
            EmitLabel(a);
            return temporaryExpressionNode;
        }
    }
}
