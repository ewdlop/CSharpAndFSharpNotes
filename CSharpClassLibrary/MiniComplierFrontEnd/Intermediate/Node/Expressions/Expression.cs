using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{ 
    public abstract record Expression(Token OperationToken, TypeToken TypeToken) : Node, IExpression
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
                Emit($"if {test} goto L{t}");
                Emit($"if goto L{f}");
            }
            else if (t != 0)
            {
                Emit($"if {test} goto L{t}");
            }
            else if (f != 0)
            {
                Emit($"iffalse {test} goto L{f}");
            }
        }
        public override string ToString() => OperationToken.ToString();
    }
}
