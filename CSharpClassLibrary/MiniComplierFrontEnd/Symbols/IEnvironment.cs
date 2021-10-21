using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Token;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Symbols
{
    public interface IEnvironment
    {
        public IdExpressionNode Get(Token token);
        public void Put(Token token, IdExpressionNode idExpressionNode);
    }
}
