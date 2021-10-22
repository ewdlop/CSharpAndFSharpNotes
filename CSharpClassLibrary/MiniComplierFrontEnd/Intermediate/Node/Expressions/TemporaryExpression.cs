using CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Tokens;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{
    public record TemporaryExpression(TypeToken TypeToken) : Expression(WordToken.TEMP, TypeToken)
    {
        public int Number { get; } = ++Count;
        private static int Count { get; set; }
        public override string ToString() => $"t{Number}";
    }
}
