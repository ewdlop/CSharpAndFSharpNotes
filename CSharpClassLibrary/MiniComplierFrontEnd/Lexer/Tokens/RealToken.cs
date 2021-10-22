namespace CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Tokens
{
    public record RealToken(float Value): Token(TokenTag.REAL)
    {
        public override string ToString() => Value.ToString();
    }
}
