using System;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;

public record Token(int Tag)
{
    public override string ToString() => Convert.ToChar(Tag).ToString();

}
