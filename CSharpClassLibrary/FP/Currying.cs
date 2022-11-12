namespace CSharpClassLibrary.FP;

public static class Currying
{
    public static Func<R> Partial<T, R>(this Func<T, R> function, T arg)
    {
        return () => function(arg);
    }
    public static Func<T, Func<R>> Curry<T, R>(this Func<T, R> function)
    {
        return arg => () => function(arg);
    }
    
    public static Func<T1, Func<T2, TR>> Curry<T1, T2, TR>(this Func<T1, T2, TR> func) =>
        p1 => p2 => func(p1, p2);
}


public class ThreadSafeRandom : Random
{
    private ThreadLocal<Random> random = new ThreadLocal<Random>(() => new Random(MakeRandomSeed()));
    public override int Next() => random.Value.Next();
    public override int Next(int maxValue) =>
    random.Value.Next(maxValue);
    public override int Next(int minValue, int maxValue) =>
    random.Value.Next(minValue, maxValue);
    public override double NextDouble() => random.Value.NextDouble();
    public override void NextBytes(byte[] buffer) =>
    random.Value.NextBytes(buffer);
    static int MakeRandomSeed() =>
    Guid.NewGuid().ToString().GetHashCode();
}