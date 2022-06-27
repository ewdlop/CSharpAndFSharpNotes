using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Emitters;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Statements;

public class SetElementStatement: Statement
{
    private readonly IdExpression _arrayExpression;
    private readonly IExpression _indexExpression;
    private readonly IExpression _expression;
    public SetElementStatement(AccessingOperationExpression accessingOperationExpression, Expression expression, Emitter node):base(node)
    {
        _arrayExpression = accessingOperationExpression.ArrayExpression;
        _indexExpression = accessingOperationExpression.IndexExpression;
        _expression = expression;
        if(Check(accessingOperationExpression.TypeToken,_expression.TypeToken) == null)
        {
            _emitter.Error("Type Error");
        }
    }
    public static TypeToken Check(TypeToken typeToken1, TypeToken typeToken2)
    {
        if (typeToken1 is ArrayTypeToken || typeToken2 is ArrayTypeToken)
        {
            return null;
        }
        else if (typeToken1 == typeToken2)
        {
            return typeToken2;
        }
        else if (TypeToken.IsNumeric(typeToken1) && TypeToken.IsNumeric(typeToken2))
        {
            return typeToken2;
        }
        else
        {
            return null;
        }
    }
    public override void Generate(int begin, int after) => _emitter.Emit($"{_arrayExpression} [ {_indexExpression.Reduce()} ] = {_expression.Reduce()}");
}
