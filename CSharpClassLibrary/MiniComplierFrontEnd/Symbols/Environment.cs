using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;
using System.Collections.Generic;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Symbols
{
    public class Environment : IEnvironment
    {
        private readonly IDictionary<Token, IdExpression> _tokenIdExpression;
        public Environment()
        {
            _tokenIdExpression = new Dictionary<Token, IdExpression>();
        }
        protected Environment Previous { get; init; }
        public IdExpression Get(Token token)
        {
            for (Environment environment = this; environment != null; environment = environment.Previous)
            {
                if(_tokenIdExpression.TryGetValue(token, out IdExpression Id))
                {
                    return Id;
                }
            }
            return null;
        }
        public void Put(Token token, IdExpression idExpressionNode) => _tokenIdExpression.Add(token, idExpressionNode);
    }
}
