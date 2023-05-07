using Fluxor;

namespace BlazorApp3.Data;

public class Effects
{
    [EffectMethod]
    public async Task HandleIncrementCounterAction(IncrementCounterAction action, IDispatcher dispatcher)
    {
        Console.WriteLine($"HandleIncrementCounterAction:{Environment.CurrentManagedThreadId}");
        await Task.Delay(1000);
        Console.WriteLine($"HandleIncrementCounterAction After delay:{Environment.CurrentManagedThreadId}");
        dispatcher.Dispatch(new DelayIncrementCounterAction());
    }
}