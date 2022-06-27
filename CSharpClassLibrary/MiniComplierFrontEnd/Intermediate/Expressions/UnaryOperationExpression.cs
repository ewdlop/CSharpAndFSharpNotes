using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Emitters;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;

public record UnaryOperationExpression(Token Token, IExpression ExpressionNode, ILabelEmitter Node)
    : OperationExpression(Token, CSharpClassLibrary.MiniComplierFrontEnd.Symbols.Tokens.TypeToken.Max(CSharpClassLibrary.MiniComplierFrontEnd.Symbols.Tokens.TypeToken.INT, ExpressionNode.TypeToken), Node)
{
    public override Expression Generate() => new UnaryOperationExpression(OperationToken, ExpressionNode.Reduce(), Node);
    public override string ToString() => $"{OperationToken} {ExpressionNode}";
}
