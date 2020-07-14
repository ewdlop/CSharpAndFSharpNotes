using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpClassLibrary
{
    public class MakeStringContent
    {
        public MakeStringState State { get;}
        public char C { get;}

        public string Concat(string s)
        {
            return s + C;
        }
    }
}
