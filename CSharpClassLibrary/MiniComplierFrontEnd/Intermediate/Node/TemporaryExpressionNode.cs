using CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Token;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{
    public interface ITemporaryExpressionNode
    {
    }
    public record TemporaryExpressionNode(TypeToken TypeToken) : ExpressionNode(WordToken.TEMP, TypeToken), ITemporaryExpressionNode
    {
        public readonly int Number = ++Count;
        private static int Count { get; set; }
        public override string ToString() => $"t{Number}";
    }
}
