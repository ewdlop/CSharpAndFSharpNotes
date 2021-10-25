using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Statements
{
    public class IfStatement : Statement
    {
        private readonly IExpression _expression;
        private readonly IStatement _statement;
        public IfStatement(IStatement statement, IExpression expression, Node node):base(node)
        {
            _statement = statement;
            _expression = expression;
            if (_expression.TypeToken != TypeToken.BOOL)
            {
                _expression.Error("Boolean required in while");
            }
        }
        public override void Generate(int b, int a)
        {
            int Label = _node.NewLabel();
            _expression.Jumping(0, a);
            _node.EmitLabel(Label);
            _statement.Generate(Label, a);
        }
    }
}
