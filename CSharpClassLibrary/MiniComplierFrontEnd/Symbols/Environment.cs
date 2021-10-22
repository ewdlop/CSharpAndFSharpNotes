using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Tokens;
using System.Collections.Generic;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Symbols
{
    public class Environment : IEnvironment
    {
        private readonly Dictionary<Token, IdExpression> _tokenIdExpressionNode;
        public Environment()
        {
            _tokenIdExpressionNode = new();
        }
        protected Environment Previous { get; init; }
        public IdExpression Get(Token token)
        {
            for (Environment environment = this; environment != null; environment = environment.Previous)
            {
                if(_tokenIdExpressionNode.TryGetValue(token, out IdExpression Id))
                {
                    return Id;
                }
            }
            return null;
        }
        public void Put(Token token, IdExpression idExpressionNode) => _tokenIdExpressionNode.Add(token, idExpressionNode);
    }
}
