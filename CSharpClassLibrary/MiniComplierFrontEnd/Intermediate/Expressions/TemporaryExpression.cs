using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Nodes;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions
{
    public record TemporaryExpression(TypeToken TypeToken, Node Node) : Expression(WordToken.TEMP, TypeToken, Node)
    {
        public int Number { get; } = ++Count;
        private static int Count { get; set; }
        public override string ToString() => $"t{Number}";
    }
}
