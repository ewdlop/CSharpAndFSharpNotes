﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpClassLibrary.RandomComplier
{
    public interface IMakeStringState
    {
        Token DoAction(MakeStringContent content, string s);
    }
}
