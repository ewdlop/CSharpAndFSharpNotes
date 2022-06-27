using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Emitters;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Statements;

public class WhileStatement : Statement
{
    public WhileStatement(Emitter node) : base(node){}
    public IExpression? Expression { get; private set; }
    public IStatement? Statement { get; private set; }
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
        int label = _emitter.NewLabel();
        _emitter.EmitLabel(label);
        Statement.Generate(label, begin);
        _emitter.Emit($"goto L{begin}");
    }
}
