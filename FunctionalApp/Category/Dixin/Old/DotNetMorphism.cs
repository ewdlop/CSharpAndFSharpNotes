using System;
using System.Diagnostics.Contracts;

namespace CSharpClassLibrary.Category.Dixin.Old;

public class DotNetMorphism<TSource, TResult> : IMorphism<TSource, TResult, DotNet>
{
    private readonly Func<TSource, TResult> function;

    public DotNetMorphism(Func<TSource, TResult> function)
    {
        this.function = function;
    }

    public DotNet Category
    {
        [Pure]
        get { return DotNet.Category; }
    }

    [Pure]
    public TResult Invoke
        (TSource source) => function(source);
}