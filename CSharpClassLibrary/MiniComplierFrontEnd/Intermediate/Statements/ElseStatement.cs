using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Statements
{
    public class ElseStatement:Statement
    {
        public readonly IExpression _expression;
        public readonly IStatement _statement1;
        public readonly IStatement _statement2;
        public ElseStatement(IExpression expression, IStatement statement1, IStatement statement2, Node node) :base(node)
        {
            _expression = expression;
            if (_expression.TypeToken != TypeToken.BOOL)
            {
                _expression.Error("Boolean required in while");
            }
            _statement1 = statement1;
            _statement2 = statement2;
        }
        public override void Generate(int b, int a)
        {
            int Label1 = _node.NewLabel();
            int Label2 = _node.NewLabel();
            _expression.Jumping(0, Label2);
            _node.EmitLabel(Label1);
            _statement1.Generate(Label1, a);
            _node.Emit($"goto L{a}");
            _node.EmitLabel(Label2);
            _statement2.Generate(Label2, a);
        }
    }
}
