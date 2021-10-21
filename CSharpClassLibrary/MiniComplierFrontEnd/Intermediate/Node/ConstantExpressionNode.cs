using CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Token;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{
    public record ConstantExpressionNode(Token Token, TypeToken TypeToken) : ExpressionNode(Token, TypeToken)
    {
        public ConstantExpressionNode(int i) : this(new NumberToken(i), TypeToken.INT) { }
        internal static readonly ConstantExpressionNode TRUE = new(WordToken.TRUE, TypeToken.BOOL), False = new(WordToken.FALSE, TypeToken.BOOL);

        public override void Jumping(int t, int f)
        {
            if (this == TRUE && t != 0) Emit("goto L" + t);
            else if (this == False && f != 0) Emit("goto L" + f);
        }
    }
}
