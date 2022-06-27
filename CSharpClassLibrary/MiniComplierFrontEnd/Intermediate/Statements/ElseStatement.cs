using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Emitters;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Statements;

public class ElseStatement:Statement
{
    public readonly IExpression _expression;
    public readonly IStatement _statement1;
    public readonly IStatement _statement2;
    public ElseStatement(IExpression expression, IStatement statement1, IStatement statement2, Emitter node) :base(node)
    {
        _expression = expression;
        if (_expression.TypeToken != TypeToken.BOOL)
        {
            _expression.Error("Boolean required in while");
        }
        _statement1 = statement1;
        _statement2 = statement2;
    }
    public override void Generate(int begin, int after)
    {
        int Label1 = Emitter.NewLabel();
        int Label2 = Emitter.NewLabel();
        _expression.Jumping(0, Label2);
        _emitter.EmitLabel(Label1);
        _statement1.Generate(Label1, after);
        _emitter.Emit($"goto L{after}");
        _emitter.EmitLabel(Label2);
        _statement2.Generate(Label2, after);
    }
}
