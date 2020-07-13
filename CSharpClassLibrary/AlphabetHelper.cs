using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CSharpClassLibrary
{
    public static class AlphabetHelper
    {
        public static Regex RegexLetter { get; } = new Regex("^[a-zA-Z]$");
        public static Regex RegexNumber { get; } = new Regex("^\\d$");
        public static Regex RegexLiteral { get; } = new Regex("^\\w$");
        public static Regex RegexOperator { get; } = new Regex("^[+-\\\\*/<>=!&|^%]$");

        public static bool IsLetter(char c)
        {
            return RegexLetter.IsMatch(c + "");
        }

        public static bool IsNumber(char c)
        {
            return RegexNumber.IsMatch(c + "");
        }

        public static bool IsLiteral(char c)
        {
            return RegexLiteral.IsMatch(c + "");
        }

        public static bool IsOperator(char c)
        {
            return RegexOperator.IsMatch(c + "");
        }
    }
}
