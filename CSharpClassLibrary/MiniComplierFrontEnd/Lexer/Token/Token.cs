namespace CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Token
{
    public record Token(int Tag)
    {
        public override string ToString() => Tag.ToString();
    }
}
