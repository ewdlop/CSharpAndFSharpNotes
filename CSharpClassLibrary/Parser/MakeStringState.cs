using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpClassLibrary
{
    public interface MakeStringState
    {
        Token DoAction(MakeStringContent content, string s);
    }
}
