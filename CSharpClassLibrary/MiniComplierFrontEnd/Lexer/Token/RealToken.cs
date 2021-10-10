namespace CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Token
{
    public record RealToken(float Value): Token(TokenTag.REAL)
    {
        public override string ToString() => Value.ToString();
    }
}
