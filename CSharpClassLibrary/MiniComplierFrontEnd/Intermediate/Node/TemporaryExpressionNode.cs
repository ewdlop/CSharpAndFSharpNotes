using CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Token;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{
    public interface ITemporaryExpressionNode
    {
        int Number { get;}
    }
    public record TemporaryExpressionNode(TypeToken TypeToken) : ExpressionNode(WordToken.TEMP, TypeToken), ITemporaryExpressionNode
    {
        public readonly int Number = ++Count;
        private static int Count { get; set; }

        int ITemporaryExpressionNode.Number => Number;

        public override string ToString() => $"t{Number}";
    }
}
