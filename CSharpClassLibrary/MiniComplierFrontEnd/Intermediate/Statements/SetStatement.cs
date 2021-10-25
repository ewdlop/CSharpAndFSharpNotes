using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Statements
{
    public class SetStatement : Statement
    {
        private readonly IdExpression _idExpression;
        private readonly IExpression _expression;
        public SetStatement(IdExpression idExpression, IExpression expression, Node node):base(node)
        {
            _idExpression = idExpression;
            _expression = expression;
            if(Check(_idExpression.TypeToken,expression.TypeToken) == null)
            {
                _node.Error("Type Error");
            }
        }
        public static TypeToken Check(TypeToken typeToken1, TypeToken typeToken2)
        {
            if (TypeToken.IsNumeric(typeToken1) && TypeToken.IsNumeric(typeToken2))
            {
                return typeToken2;
            }
            else if (typeToken1 == TypeToken.BOOL && typeToken2 == TypeToken.BOOL)
            {
                return typeToken2;
            }
            else
            {
                return null;
            }
        }
        public override void Generate(int b, int a) => _node.Emit($"{_idExpression} = {_expression.Generate()}");
    }
}