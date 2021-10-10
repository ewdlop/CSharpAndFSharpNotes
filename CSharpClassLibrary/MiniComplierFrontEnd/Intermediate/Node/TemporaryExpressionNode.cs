using CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Token;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{
    public interface ITemporaryExpressionNode
    {
        int Number { get; set; }
    }
    public record TemporaryExpressionNode(TypeToken TypeToken) : ExpressionNode(WordToken.TEMP, TypeToken), ITemporaryExpressionNode
    {
        static int Count { get; set; }
        int ITemporaryExpressionNode.Number { get; set; } = ++Count;
        public override string ToString() => $"t{(this as ITemporaryExpressionNode).Number}";
    }
}
