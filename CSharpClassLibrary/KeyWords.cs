using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpClassLibrary
{
    public static class KeyWords
    {
        public static string[] Keywords { get; } = {
            "if",
            "else",
            "for",
            "while",
            "break",
            "func",
            "return",
            "int",
            "float",
            "string",
            "boolean",
            "void"
        };

        public static HashSet<string> Set { get; } = new HashSet<string>(Keywords);
        public static bool IsKeyword(string word) {
            return Set.Contains(word);
        }
    }
}
