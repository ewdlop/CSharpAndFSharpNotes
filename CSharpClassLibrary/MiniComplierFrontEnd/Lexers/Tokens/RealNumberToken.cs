namespace CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;

public record RealNumberToken(float Value): Token(TokenTag.REAL)
{
    public override string ToString() => Value.ToString();
}
