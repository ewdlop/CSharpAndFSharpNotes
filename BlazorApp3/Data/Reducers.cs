using Fluxor;

namespace BlazorApp3.Data;

public static class Reducers
{
    [ReducerMethod]
    public static CounterState ReduceIncrementCounterAction(CounterState state, IncrementCounterAction action)
    {
        Console.WriteLine($"ReduceIncrementCounterAction:{Environment.CurrentManagedThreadId}");
        return new CounterState(clickCount: state.ClickCount + 1);
    }

    [ReducerMethod]
    public static CounterState ReduceDelayIncrementCounterAction(CounterState state, DelayIncrementCounterAction action)
    {
        Console.WriteLine($"ReduceIncrementCounterAction:{Environment.CurrentManagedThreadId}");
        return new CounterState(clickCount: state.ClickCount + 1);
    }
}
