﻿using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Symbols.Tokens;

public record ArrayTypeToken(TypeToken OfTypeToken, int Size) : TypeToken("[]", TokenTag.INDEX, Size * OfTypeToken.Width)
{
    public override string ToString() => $"[{Size}]{OfTypeToken}";
}
