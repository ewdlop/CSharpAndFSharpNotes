using System;

namespace CSharpClassLibrary.Category.Dixin;

public static partial class Functions
{
    public static Func<TSource, TResult> o<TSource, TMiddle, TResult>(
        this Func<TMiddle, TResult> function2, Func<TSource, TMiddle> function1) =>
            value => function2(function1(value));

    public static TSource Id<TSource>(TSource value) => value;
}
