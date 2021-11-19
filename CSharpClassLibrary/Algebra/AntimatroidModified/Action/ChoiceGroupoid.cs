using System;

namespace CSharpClassLibrary.Algebra.AntimatroidModified.Extension;

public class ChoiceGroupoid : IGroupoid<Action>
{
    public Action Operation(Action a, Action b)
    {
        if (DateTime.Now.Ticks % 2 == 0)
            return a;
        return b;
    }
}