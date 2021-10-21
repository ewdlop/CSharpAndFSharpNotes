using CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Token;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{
    public record LogicalExpressionNode(Token Token, ExpressionNode ExpressionNode1, ExpressionNode ExpressionNode2)
        : ExpressionNode(Token, Check(ExpressionNode1.TypeToken, ExpressionNode2.TypeToken))
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

        public override ExpressionNode Generate()
        {
            int f = NewLabel();
            int a = NewLabel();
            var temporaryExpressionNode = new TemporaryExpressionNode(TypeToken);
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
