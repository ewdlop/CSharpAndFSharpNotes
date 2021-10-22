namespace CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Tokens
{
    public record Token(int Tag)
    {
        public override string ToString() => Tag.ToString();
    }
}
