using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Token;
using System.Collections.Generic;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Symbols
{
    public interface IEnvironment
    {
        Dictionary<Token, IdExpressionNode> TokenIdExpressionNode { get; init; }
        public void Put(Token token, IdExpressionNode idExpressionNode) => TokenIdExpressionNode.Add(token, idExpressionNode);
        public IdExpressionNode Get(Token token);
    }
    public class Environment : IEnvironment
    {
        Dictionary<Token, IdExpressionNode> IEnvironment.TokenIdExpressionNode { get; init; } = new();
        protected Environment Previous { get; init; }
        public IdExpressionNode Get(Token token)
        {
            for (Environment environment = this; environment != null; environment = environment.Previous)
            {
                if((environment as IEnvironment).TokenIdExpressionNode.TryGetValue(token, out IdExpressionNode Id))
                {
                    return Id;
                }
            }
            return null;
        }
    }
}
