using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;
using CSharpClassLibrary.MiniComplierFrontEnd.Symbols.Tokens;
using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Emitters;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;

public record IdExpression(Token Token, TypeToken TypeToken, int Offset, ILabelEmitter Node) : Expression(Token, TypeToken, Node)
{
    public override string ToString() => base.ToString();
}
