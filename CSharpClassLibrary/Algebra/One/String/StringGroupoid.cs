﻿namespace CSharpClassLibrary.Algebra.One.String
{
    public class StringGroupoid : IGroupoid<string>
    {
        public string Operation(string a, string b) => a + b;
    }
}