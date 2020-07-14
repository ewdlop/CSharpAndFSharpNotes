using CSharpClassLibrary;
using System;
using Xunit;

namespace XUnitTestProject
{
    public class UnitTest1
    {
        [Fact]
        public void Test_VarOrKeyword()
        {
            PeekableEnumerableAdapter<char> it1 = new PeekableEnumerableAdapter<char>("if abc");
            PeekableEnumerableAdapter<char> it2 = new PeekableEnumerableAdapter<char>("true abc");
            Token token1 = Token.MakeVarOrKeyword(it1);
            Token token2 = Token.MakeVarOrKeyword(it2);
            Assert.Equal(TokenType.KEYWORD, token1.Type);
            Assert.Equal("if", token1.Value);
            Assert.Equal(TokenType.BOOLEAN, token2.Type);
            Assert.Equal("true", token2.Value);
            it1.Next();
            Token token3 = Token.MakeVarOrKeyword(it1);
            Assert.Equal(TokenType.VARIABLE, token3.Type);
            Assert.Equal("abc", token3.Value);
        }
    }
}
