namespace CSharpClassLibrary.FP;

public static class GenericExtension
{
    public static R Using<T, R>(this T item, Func<T, R> func) where T : IDisposable
    {
        using (item)
            return func(item);
    }

    public static async Task<R> AwaitUsing<T, R>(this T item, Func<T, R> func) where T : IAsyncDisposable
    {
        await using (item)
            return func(item);
    }
}

