using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Statements
{
    public class WhileStatement:Statement
    {
        public WhileStatement(Node node) : base(node){}
        public IExpression Expression { get; private set; }
        public IStatement Statement { get; private set; }
        public override void Init(IExpression expression, IStatement statement)
        {
            Expression = expression;
            Statement = statement;
            if(Expression.TypeToken != TypeToken.BOOL)
            {
                Expression.Error("Boolean required in while");
            }
        }
        public override void Generate(int begin, int after)
        {
            After = after;
            Expression.Jumping(0, after);
            int label = _node.NewLabel();
            _node.EmitLabel(label);
            Statement.Generate(label, begin);
            _node.Emit($"goto L{begin}");
        }
    }
}
