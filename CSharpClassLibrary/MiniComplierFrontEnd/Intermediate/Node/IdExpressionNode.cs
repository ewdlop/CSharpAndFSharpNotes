using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Token;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{
    public record IdExpressionNode(Token Token, TypeToken TypeToken, int Offset) : ExpressionNode(Token, TypeToken) { }
}
