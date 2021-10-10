using System;

namespace CSharpClassLibrary.RandomComplier
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

    public class Token {
        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }

        public TokenType Type { get; set; }

        public string Value { get; set; }
        new public string ToString => string.Format("type {0}, value {1}", Type, Value);

        public bool IsScalar() {
            return Type == TokenType.INTEGER || Type == TokenType.FLOAT
                    || Type == TokenType.STRING || Type == TokenType.BOOLEAN;
        }

        public bool IsVariable => Type == TokenType.VARIABLE;

        public bool IsType() {
            return Value.Equals("bool") || Value.Equals("int")
                    || Value.Equals("float") || Value.Equals("void")
                    || Value.Equals("string");
        }

        public static Token ParsingUsingIterator(PeekableEnumerableAdapter<char> iterator) {
            string s = "";
            while (iterator.HasNext) {
                char lookahead = iterator.Peek();
                if (AlphabetHelper.IsLetter(lookahead)) {
                    s += lookahead;
                } else {
                    break;
                }
                iterator.Next();
            }
            if (KeyWords.IsKeyword(s)) {
                return new Token(TokenType.KEYWORD, s);
            }
            if (s.Equals("true") || s.Equals("false")) {
                return new Token(TokenType.BOOLEAN, s);
            }
            return new Token(TokenType.VARIABLE,s);
        }

        public static Token MakeString(PeekableEnumerableAdapter<char> iterator) {
            string s = "";
            IMakeStringState startState = new StartMakeStringState();
            MakeStringContent content = new MakeStringContent(startState);
            while (iterator.HasNext) {
                char c = iterator.Next();
                content.C = c;
                Token token = content.State.DoAction(content, s + c);
                if (token != null) {
                    return token;
                } else {
                    s = content.Concat(s);
                }
            }
            throw new LexicalException("Unexpected error");
        }
    }
}
