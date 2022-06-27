using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Emitters;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Statements;

public class Statement: IStatement
{
    internal static readonly IStatement NullStatement = new Statement(EmitterFactory.GetDummyEmitter());
    internal static IStatement EnclosingStatement = NullStatement;
    protected readonly ILabelEmitter _emitter;
    public Statement(ILabelEmitter emitter)
    {
        _emitter = emitter;
    }
    public int After { get; protected set; }
    public virtual void Generate(int begin, int after) { }

    public ILabelEmitter Emitter => _emitter;

    public virtual void Init(IExpression expression, IStatement statement) { }
}
