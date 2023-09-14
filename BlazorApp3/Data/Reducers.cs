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

public abstract record TestState<T>(T ClickCount);


[FeatureState]
public record ConcreteTestState : TestState<int>
{
    public ConcreteTestState() : base(0)
    {
    }

    public ConcreteTestState(int Count) : base(Count)
    {

    }

}

public record CreateAction<T>(Func<TestState<T>, TestState<T>> Action);

public static class Reducers2
{

    [ReducerMethod]
    public static TestState<int> ReduceIncrementCounterAction2<T>(TestState<int> state, CreateAction<int> Create)
    {
        Console.WriteLine($"ReduceIncrementCounterAction2:{Environment.CurrentManagedThreadId}");
        return Create.Action(state);
    }

    //[ReducerMethod]
    //public static TestState<int> ReduceIncrementCounterAction2<T>(TestState<int> state, CreateAction<int> Create)
    //{
    //    Console.WriteLine($"ReduceIncrementCounterAction2:{Environment.CurrentManagedThreadId}");
    //    return Create.Action(state);
    //}

    [ReducerMethod]
    public static TestState<T> ReduceIncrementCounterAction2<T>(TestState<T> state, CreateAction<T> Create)
    {
        Console.WriteLine($"ReduceIncrementCounterAction2:{Environment.CurrentManagedThreadId}");
        return Create.Action(state);
    }
}