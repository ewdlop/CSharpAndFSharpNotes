namespace CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Tokens
{
    public record NumberToken(int Value) : Token(TokenTag.NUMBER)
    {
        public override string ToString() => Value.ToString();
    }
}
