using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Emitters;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;

public record ArithmeticOperationExpression(Token Token, IExpression Expression1, IExpression Expression2, ILabelEmitter Node)
    : OperationExpression(Token, CSharpClassLibrary.MiniComplierFrontEnd.Symbols.Tokens.TypeToken.Max(Expression1.TypeToken, Expression2.TypeToken), Node)
{
    public override Expression Generate() => new ArithmeticOperationExpression(OperationToken, Expression1.Reduce(), Expression2.Reduce(), Node);
    public override string ToString() => $"{Expression1} {OperationToken} {Expression2}";
}
