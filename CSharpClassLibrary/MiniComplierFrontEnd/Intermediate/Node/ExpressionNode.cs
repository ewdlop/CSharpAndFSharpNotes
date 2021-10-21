using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Token;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{
    public interface IExpressionEmitable
    {
        void EmitJumps(string test, int t, int f);
    }
    public interface IExpressionNode : IExpressionEmitable
    {
        ExpressionNode Generate();
        ExpressionNode Reduce();
        void Jumping(int t, int f);
    }
    public abstract record ExpressionNode(Token OperationToken, TypeToken TypeToken) : Node, IExpressionNode
    {
        public virtual ExpressionNode Generate() => this;
        public virtual ExpressionNode Reduce() => this;
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
            else if (t != 0) Emit($"if {test} goto L{t}");
            else if (f != 0) Emit($"iffalse {test} goto L{f}");
            else
            {

            }
        }
        public override string ToString() => OperationToken.ToString();
    }
}
