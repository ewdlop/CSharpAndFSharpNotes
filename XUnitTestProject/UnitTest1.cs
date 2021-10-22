using CSharpClassLibrary;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace XUnitTestProject
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper _output;

        public UnitTest1(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test_VarOrKeyword()
        {
            //PeekableEnumerableAdapter<char> it1 = new PeekableEnumerableAdapter<char>("if abc");
            //PeekableEnumerableAdapter<char> it2 = new PeekableEnumerableAdapter<char>("true abc");
            //Token token1 = Token.ParsingUsingIterator(it1);
            //Token token2 = Token.ParsingUsingIterator(it2);
            //Assert.Equal(TokenType.KEYWORD, token1.Type);
            //Assert.Equal("if", token1.Value);
            //Assert.Equal(TokenType.BOOLEAN, token2.Type);
            //Assert.Equal("true", token2.Value);
            //it1.Next();
            //Token token3 = Token.ParsingUsingIterator(it1);
            //Assert.Equal(TokenType.VARIABLE, token3.Type);
            //Assert.Equal("abc", token3.Value);
        }
        
        [Fact]
        public void Test_MakeString()
        {
            //string[] tests = {
            //"\"123\"",
            //"\'123\'"};

            //tests.Select(s => {
            //    try {
            //        return Token.MakeString(new PeekableEnumerableAdapter<char>(s));
            //    }
            //    catch (LexicalException e) {
            //        _output.WriteLine(e.StackTrace);
            //        return null;
            //    }
            //}).ToList().ForEach(t => Assert.Equal(TokenType.STRING, t.Type));
        }
    }
}