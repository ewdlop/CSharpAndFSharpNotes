namespace CSharpClassLibrary.MiniComplierFrontEnd.Lexers.Tokens;

public record WordToken(string Lexeme, int Tag) : Token(Tag)
{
    public override string ToString() => Lexeme;
    internal static readonly WordToken
        AND = new("&&", TokenTag.AND),
        OR = new("||", TokenTag.OR),
        EQUAL = new("==", TokenTag.EQUAL),
        NOT_EQUAL = new("!=", TokenTag.NOT_EQUAL ),
        LESS_THAN = new("<", TokenTag.LESS_THAN ),
        LESS_OR_EQUAL = new("<=", TokenTag.LESS_OR_EQUAL),
        GREATER_THAN = new(">", TokenTag.GREATER_THAN),
        GREATER_OR_EQUAL = new(">=", TokenTag.GREATER_OR_EQUAL),
        MINUS = new("minus", TokenTag.MINUS),
        TRUE = new("true", TokenTag.TRUE),
        FALSE = new("false", TokenTag.FALSE),
        TEMP = new("temp", TokenTag.TEMP),
        NULL = new("null", TokenTag.NULL);
}
