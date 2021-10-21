using CSharpClassLibrary.MiniComplierFrontEnd.Lexer.Token;

namespace CSharpClassLibrary.MiniComplierFrontEnd.Symbols
{
    public record TypeToken(string Lexeme, int Tag, int Width): WordToken(Lexeme, Tag)
    {
        internal static readonly TypeToken
            INT = new("int", TokenTag.PRIMITIVE, 4),
            FLOAT = new("float", TokenTag.PRIMITIVE, 8),
            CHAR = new("char", TokenTag.PRIMITIVE, 1),
            BOOL = new("bool", TokenTag.PRIMITIVE, 1);
        public static bool IsNumeric(TypeToken basicTypeToken) => basicTypeToken == INT
            || basicTypeToken == FLOAT
            || basicTypeToken == CHAR;

        public static TypeToken Max(TypeToken typeToken1, TypeToken typeToken2)
        {
            if (!IsNumeric(typeToken1) || !IsNumeric(typeToken2))
            {
                return null;
            }
            else if (typeToken1.Equals(FLOAT) || typeToken2.Equals(FLOAT))
            {
                return FLOAT;
            }
            else if (typeToken1.Equals(INT) || typeToken2.Equals(INT))
            {
                return INT;
            }
            else
            {
                return CHAR;
            }
        }
    }
}
