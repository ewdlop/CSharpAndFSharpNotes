namespace ConsoleApp1.Portraits.Common;

public static class RandomNumberGenerator
{
    [ThreadStatic]
    private static Random _random;
    public static Random Random => _random ??= new Random((int)((1 + Environment.CurrentManagedThreadId) * DateTime.UtcNow.Ticks));
    //Default Random.Next is not thread-safe
    public static int GetRandomNumber(int min, int max) => Random.Next(Math.Min(min, max), Math.Max(max, min));
}