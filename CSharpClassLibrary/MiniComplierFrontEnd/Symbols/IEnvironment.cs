using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Symbols
{
    public interface IEnvironment
    {
        public IdExpression Get(Token token);
        public void Put(Token token, IdExpression idExpressionNode);
    }
}
