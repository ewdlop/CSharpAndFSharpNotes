using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Symbols
{
    public interface IEnvironment : IReadOnlyEnvironment
    {
        public void Put(Token token, IdExpression idExpression);
    }
}
