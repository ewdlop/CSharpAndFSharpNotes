using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Emitters;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Statements;

public class SequenceStatement: Statement
{
    private readonly IStatement _statement1;
    private readonly IStatement _statement2;
    public SequenceStatement(IStatement statement1,
                             IStatement statement2,
                             Emitter node) :base(node)
    {
        _statement1 = statement1;
        _statement2 = statement2;
    }
    public override void Generate(int begin, int after)
    {
        if(_statement1 == NullStatement)
        {
            _statement2.Generate(begin, after);
        }
        else if( _statement2 == NullStatement)
        {
            _statement1.Generate(begin, after);
        }
        else
        {
            int label = _emitter.NewLabel();
            _statement1.Generate(begin, label);
            _emitter.EmitLabel(label);
            _statement2.Generate(label, after);
        }
    }
}
