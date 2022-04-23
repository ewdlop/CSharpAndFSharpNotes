using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Statements;

public class DoStatement:Statement
{
    public DoStatement(Node node) : base(node){}
    public IExpression Expression { get; private set; }
    public IStatement Statement { get; private set; }
    public override void Init(IExpression expression, IStatement statement)
    {
        Expression = expression;
        Statement = statement;
        if (Expression.TypeToken != TypeToken.BOOL)
        {
            Expression.Error("Boolean required in while");
        }
    }
    public override void Generate(int begin, int after)
    {
        After = after;
        int label = _node.NewLabel();
        Statement.Generate(begin, label);
        _node.EmitLabel(label);
        Expression.Jumping(begin, 0);
    }
}
