using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{
    public record OperationExpression(Token Token, TypeToken TypeToken) : Expression(Token,TypeToken)
    {
        public override Expression Reduce()
        {
            var expression = base.Generate();
            var tempExpressionNode = new TemporaryExpression(TypeToken);
            Emit($"{tempExpressionNode} = {expression}");
            return null;
        }
    }
}
