namespace CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens
{
    public record Token(int Tag)
    {
        public override string ToString() => Tag.ToString();
    }
}
