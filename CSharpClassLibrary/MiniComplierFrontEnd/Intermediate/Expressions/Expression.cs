using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols.Tokens;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Emitters;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;

public record Expression(Token OperationToken, TypeToken TypeToken, ILabelEmitter Node) : IExpression
{
    public virtual IExpression Generate() => this;
    public virtual IExpression Reduce() => this;
    public virtual void Jumping(int t, int f)
    {
        EmitJumps(ToString(), t, f);
    }
    public virtual void EmitJumps(string test, int t, int f)
    {
        if (t != 0 && f != 0)
        {
            Node.Emit($"if {test} goto L{t}");
            Node.Emit($"goto L{f}");
        }
        else if (t != 0)
        {
            Node.Emit($"if {test} goto L{t}");
        }
        else if (f != 0)
        {
            Node.Emit($"iffalse {test} goto L{f}");
        }
    }

    public void Error(string message) => Node.Error(message);
    public override string ToString() => OperationToken.ToString();
}
