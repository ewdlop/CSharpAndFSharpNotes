using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Emitters;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Statements;

public class IfStatement : Statement
{
    private readonly IExpression _expression;
    private readonly IStatement _statement;
    public IfStatement(IExpression expression, IStatement statement,  Emitter node):base(node)
    {
        _statement = statement;
        _expression = expression;
        if (_expression.TypeToken != TypeToken.BOOL)
        {
            _expression.Error("Boolean required in while");
        }
    }
    public override void Generate(int begin, int after)
    {
        int Label = Emitter.NewLabel();
        _expression.Jumping(0, after);
        _emitter.EmitLabel(Label);
        _statement.Generate(Label, after);
    }
}
