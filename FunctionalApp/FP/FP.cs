using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CSharpClassLibrary.FP;

public static class FP
{
    public static Func<T, R> Memoize<T,R>(this Func<T, R> func) where T : notnull, IComparable
    {
        Dictionary<T, R> cache = new(); // auto-generate persistent locals ??
        return arg =>
        {
            if (cache.ContainsKey(arg))
                return cache[arg];
            return cache[arg] = func(arg); //call
        };
    }

    public static Func<T, R> ThreadSafeMemoize<T, R>(this Func<T, R> func) where T : notnull, IComparable
    {
        var cache = new ConcurrentDictionary<T, R>();
        return argument => cache.GetOrAdd(argument, a=>func(a));
    }

    public static Func<T, R> MemoizeLazyThreadSafe<T, R>(this Func<T, R> func) where T : notnull, IComparable
    {
        ConcurrentDictionary<T, Lazy<R>> cache = new();
        return arg => cache.GetOrAdd(arg, a =>new Lazy<R>(() => func(a))).Value;
    }

}
