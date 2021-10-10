using CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Token;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{
    public record ArithmeticOperationExpressionNode(Token Token, ExpressionNode ExpressionNode1, ExpressionNode ExpressionNode2)
        : OperationExpressionNode(Token, Symbols.TypeToken.Max(ExpressionNode1.TypeToken, ExpressionNode2.TypeToken))
    {
        public override ExpressionNode Generate() => new ArithmeticOperationExpressionNode(OperationToken, ExpressionNode1.Reduce(), ExpressionNode2.Reduce());
        public override string ToString() => $"{ExpressionNode1} {OperationToken} {ExpressionNode2}";
    }
}