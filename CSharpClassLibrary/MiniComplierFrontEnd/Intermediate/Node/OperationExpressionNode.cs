using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Token;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{
    public record OperationExpressionNode(Token Token, TypeToken TypeToken) : ExpressionNode(Token,TypeToken)
    {
        public override ExpressionNode Reduce()
        {
            var expression = base.Generate();
            var tempExpressionNode = new TemporaryExpressionNode(TypeToken);
            Emit($"{tempExpressionNode} = {expression}");
            return null;
        }
    }
}
