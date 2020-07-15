namespace CSharpClassLibrary
{
    public class DoubleQuotesMakeStringState : IMakeStringState
    {
        public Token DoAction(MakeStringContent content, string s)
        {
            if (content.C == '"') {
                return new Token(TokenType.STRING, s);
            } else {
                return null;
            }
        }
    }
}
