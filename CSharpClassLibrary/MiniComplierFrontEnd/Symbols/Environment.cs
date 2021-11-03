using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Expressions;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;
using System.Collections.Generic;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Symbols
{
    public class Environment : IEnvironment
    {
        private readonly Dictionary<Token, IdExpression> _tokenIdExpression;
        public Environment(IEnvironment environment)
        {
            _tokenIdExpression = new Dictionary<Token, IdExpression>();
            Previous = environment;
        }
        public Environment()//not sure here
        {
            _tokenIdExpression = new Dictionary<Token, IdExpression>();
        }
        public IReadOnlyDictionary<Token, IdExpression> TokenIdExpression => _tokenIdExpression;
        protected IEnvironment Previous { get; init; }
        public IReadOnlyEnvironment PreviousEnvironment => Previous;
        public IdExpression Get(Token token)
        {
            for (IReadOnlyEnvironment environment = this; environment != null; environment = environment.PreviousEnvironment)
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
