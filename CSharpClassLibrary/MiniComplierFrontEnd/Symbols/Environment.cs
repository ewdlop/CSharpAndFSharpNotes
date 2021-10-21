﻿using CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Token;
using System.Collections.Generic;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Symbols
{
    public class Environment : IEnvironment
    {
        private readonly Dictionary<Token, IdExpressionNode> _tokenIdExpressionNode;
        public Environment()
        {
            _tokenIdExpressionNode = new();
        }
        protected Environment Previous { get; init; }
        public IdExpressionNode Get(Token token)
        {
            for (Environment environment = this; environment != null; environment = environment.Previous)
            {
                if(_tokenIdExpressionNode.TryGetValue(token, out IdExpressionNode Id))
                {
                    return Id;
                }
            }
            return null;
        }
        public void Put(Token token, IdExpressionNode idExpressionNode) => _tokenIdExpressionNode.Add(token, idExpressionNode);
    }
}
