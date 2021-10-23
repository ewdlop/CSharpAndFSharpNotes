using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Symbols
{
    public interface IEnvironment
    {
        public IdExpression Get(Token token);
        public void Put(Token token, IdExpression idExpression);
    }
}
