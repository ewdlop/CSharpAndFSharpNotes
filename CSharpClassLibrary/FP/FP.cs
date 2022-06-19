using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpClassLibrary.FP;

public static class FP<T, R> where T : notnull, IComparable
{
    public static Func<T, R> Memoize<T, R>(Func<T, R> func) 
    {
        Dictionary<T, R> cache = new Dictionary<T, R>(); // auto-generate persistent locals ??
        return arg =>
        {
            if (cache.ContainsKey(arg))
                return cache[arg];
            return cache[arg] = func(arg); //call
        };
    }

    public static Func<A, R> ThreadSafeMemoize<A, R>(Func<A, R> func)
    {
        var cache = new ConcurrentDictionary<A, R>();
        return argument => cache.GetOrAdd(argument, a=>func(a));
    }

    public static Func<T, R> MemoizeLazyThreadSafe<T, R>(Func<T, R> func)
    {
        ConcurrentDictionary<T, Lazy<R>> cache = new ConcurrentDictionary<T, Lazy<R>>();
        return arg => cache.GetOrAdd(arg, a =>new Lazy<R>(() => func(a))).Value;
    }

}
