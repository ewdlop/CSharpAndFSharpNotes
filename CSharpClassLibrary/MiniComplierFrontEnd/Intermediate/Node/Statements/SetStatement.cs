using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node.Statements
{
    public record SetStatement(IdExpression IdExpression, IExpression Expression) : Statement
    {
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
        public override void Generate(int b, int a) => Emit($"{IdExpression} = {Expression.Generate()}");
    }
}