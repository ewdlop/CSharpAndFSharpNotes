namespace CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Token
{
    public record NumberToken(int Value) : Token(TokenTag.NUMBER)
    {
        public override string ToString() => Value.ToString();
    }
}
