namespace CSharpClassLibrary.FP;

public struct Result<T>
{
    public T? Ok { get; init; }
    public Exception? Error { get; }
    public bool IsFailed { get => Error != null; }
    public bool IsOk => !IsFailed;
    public Result(T? ok)
    {
        Ok = ok;
        Error = default;
    }
    public Result(Exception? error)
    {
        Error = error;
        Ok = default;
    }

    public R Match<R>(Func<T?, R> okMap, Func<Exception?, R> failureMap)
        => IsOk ? okMap(Ok) : failureMap(Error);

    public void Match(Action<T?> okAction, Action<Exception?> errorAction)
    {   
        if (IsOk)
        {
            okAction(Ok);
        }
        else
        {
            errorAction(Error);
        }
    }

    public static implicit operator Result<T>(T? ok) =>
        new(ok);
    public static implicit operator Result<T>(Exception? error) =>
        new(error);
    //public static implicit operator Result<T>(Result<T>.Ok ok) =>
    //    new(ok.Value);
    //public static implicit operator Result<T>(Result.Failure error) =>
    //    new(error.Error);
}

public static class ResultExtensions
{
    public static async Task<Result<T>> TryCatch<T>(Func<Task<T>> func)
    {
        try
        {
            return await func();
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
    static async Task<Result<R>> SelectMany<T, R>(this Task<Result<T>> resultTask,
        Func<T?, Task<Result<R>>> func)
    {
        Result<T> result = await resultTask.ConfigureAwait(false);
        if (result.IsFailed)
            return result.Error;
        return await func(result.Ok);
    }
    static async Task<Result<R>> Select<T, R>(this Task<Result<T>> resultTask,
        Func<T?, Task<R?>> func)
    {
        Result<T> result = await resultTask.ConfigureAwait(false);
        if (result.IsFailed)
            return result.Error;
        return await func(result.Ok).ConfigureAwait(false);
    }
    static async Task<Result<R>> Match<T, R>(this Task<Result<T>> resultTask,
         Func<T?, Task<R?>> actionOk, Func<Exception?, Task<R?>> actionError)
    {
        Result<T> result = await resultTask.ConfigureAwait(false);
        if (result.IsFailed)
            return await actionError(result.Error);
        return await actionOk(result.Ok);
    }
}