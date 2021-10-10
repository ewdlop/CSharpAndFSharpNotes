using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpClassLibrary.RandomComplier
{
    public class SingleQuotesMakeStringState : IMakeStringState
    {
        public Token DoAction(MakeStringContent content, string s)
        {
            if (content.C == '\'') {
                return new Token(TokenType.STRING, s);
            } else {
                return null;
            }
        }
    }
}
