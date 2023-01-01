using Fluxor;

namespace BlazorApp1.Store;

[FeatureState]
public record CountState(int ClickCount)
{
    public CountState() : this(0) { }
}

[FeatureState]
public record CountState2(int ClickCount)
{
    public CountState2() : this(0) { }
}

public static class Reducers
{
    [ReducerMethod]
    public static CountState2 ReduceIncrementCountAction(CountState2 state, PostIncrementCountAction action)
    {
        return state with { ClickCount = state.ClickCount + 1 };
    }

    [ReducerMethod]
    public static CountState ReduceIncrementCountAction(CountState state, IncrementCountAction action)
    {
        return state with { ClickCount = state.ClickCount + 1 };
    }

    [ReducerMethod]
    public static CountState2 ReduceIncrementCountAction(CountState2 state, IncrementCountAction action)
    {
        return state with { ClickCount = state.ClickCount + 1 };
    }

    [ReducerMethod]
    public static CountState ReduceIncrementCountAction2(CountState state, IncrementCountAction action)
    {
        return state with { ClickCount = state.ClickCount + 1 };
    }
}

public static class Reducers2
{
    [ReducerMethod]
    public static CountState ReduceIncrementCountAction3(CountState state, IncrementCountAction action)
    {
        return state with { ClickCount = state.ClickCount + 1 };
    }
}
public class Effects
{
    [EffectMethod]
    public async Task HandleIncrementCountAction(IncrementCountAction action, IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new IncrementCountAction());
    }

    [EffectMethod]
    public async Task HandleIncrementCountAction(PostIncrementCountAction action, IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new IncrementCountAction());
    }
}

public record IncrementCountAction() { }
public record PostIncrementCountAction() { }