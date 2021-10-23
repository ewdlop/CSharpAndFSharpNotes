using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node.Statements
{
    public record SetElementStatement(IdExpression ArrayExpression, IExpression IndexExpression, IExpression Expression) : Statement
    {
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
        public override void Generate(int b, int a) => Emit($"{ArrayExpression} [ {IndexExpression.Reduce()} ] = {Expression.Reduce()}");
    }
}