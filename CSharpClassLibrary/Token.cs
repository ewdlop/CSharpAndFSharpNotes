using System;

namespace CSharpClassLibrary
{
    public enum TokenType
    {
        KEYWORD,
        VARIABLE,
        OPERATOR,
        BRACKET,
        INTEGER,
        STRING,
        FLOAT,
        BOOLEAN
    }

    public class Token
    {
        public TokenType Type { get; set; }

        public string Value { get; set; }
        new public string ToString => string.Format("type {0}, value {1}", Type, Value);

        public bool IsScalar() {
            return Type == TokenType.INTEGER || Type == TokenType.FLOAT
                    || Type == TokenType.STRING || Type == TokenType.BOOLEAN;
        }

        public bool IsVariable() {
            return Type == TokenType.VARIABLE;
        }

        public bool IsType() {
            return Value.Equals("bool") || Value.Equals("int")
                    || Value.Equals("float") || Value.Equals("void")
                    || Value.Equals("string");
        }
    }
}
