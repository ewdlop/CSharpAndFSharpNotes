using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Emitters;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;

public record TemporaryExpression(TypeToken TypeToken, ILabelEmitter Node) : Expression(WordToken.TEMP, TypeToken, Node)
{
    public int Number { get; } = ++Count;
    private static int Count { get; set; }
    public override string ToString() => $"t{Number}";
}
