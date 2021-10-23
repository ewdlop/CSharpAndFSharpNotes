using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Symbols
{
    public record ArrayTypeToken(TypeToken TypeToken, int Size): TypeToken("[]", TokenTag.INDEX, Size * TypeToken.Width)
    {
        public override string ToString() => $"[{Size}]{TypeToken}";
    }
}
