using System;

namespace CSharpClassLibrary.Algebra.AntimatroidModified.Extension;

public class SequenceGroupoidWithUnity : IGroupoid<Action>
{
    public Action Identity
    {
        get { return () => { }; }
    }

    public Action Operation(Action a, Action b)
    {
        return () => { a(); b(); };
    }
}
