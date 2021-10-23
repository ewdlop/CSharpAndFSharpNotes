﻿using CSharpClassLibrary.MiniComplierFrontEnd.Symbols;
using CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Intermediate.Node
{
    public record IdExpression(Token Token, TypeToken TypeToken, int Offset) : Expression(Token, TypeToken) { }
}
