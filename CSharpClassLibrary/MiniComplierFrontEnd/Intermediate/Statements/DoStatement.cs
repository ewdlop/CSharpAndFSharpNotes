using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Emitters;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Statements;

public class DoStatement:Statement
{
    public DoStatement(Emitter node) : base(node){}
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
        int label = _emitter.NewLabel();
        Statement.Generate(begin, label);
        _emitter.EmitLabel(label);
        Expression.Jumping(begin, 0);
    }
}
