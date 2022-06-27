using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Emitters;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;

public record ConstantExpression(Token Token, TypeToken TypeToken, ILabelEmitter Node)
    : Expression(Token, TypeToken, Node)
{
    public ConstantExpression(int i, Emitter Node) : this(new NumberToken(i), TypeToken.INT, Node) { }
    internal static readonly ConstantExpression
            TRUE = new(WordToken.TRUE, TypeToken.BOOL, EmitterFactory.GetDummyEmitter()),
            FALSE = new(WordToken.FALSE, TypeToken.BOOL, EmitterFactory.GetDummyEmitter());
    public override void Jumping(int t, int f)
    {
        if (this == TRUE && t != 0) Node.Emit("goto L" + t);
        else if (this == FALSE && f != 0) Node.Emit("goto L" + f);
    }

    public override string ToString() => base.ToString();
}
