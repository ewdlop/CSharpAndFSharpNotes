using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions
{
    public record ConstantExpression(Token Token, TypeToken TypeToken, INode Node)
        : Expression(Token, TypeToken, Node)
    {
        public ConstantExpression(int i, Node Node) : this(new NumberToken(i), TypeToken.INT, Node) { }
        internal static readonly ConstantExpression
                TRUE = new(WordToken.TRUE, TypeToken.BOOL, NodeFactory.GetDummyNode()),
                False = new(WordToken.FALSE, TypeToken.BOOL, NodeFactory.GetDummyNode());
        public override void Jumping(int t, int f)
        {
            if (this == TRUE && t != 0) Node.Emit("goto L" + t);
            else if (this == False && f != 0) Node.Emit("goto L" + f);
        }
    }
}
