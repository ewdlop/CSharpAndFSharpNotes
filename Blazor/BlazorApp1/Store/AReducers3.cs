using Fluxor;

namespace BlazorApp1.Store;

public static class AReducers3
{
    [ReducerMethod]
    public static CountState ReduceIncrementCountAction3(CountState state, IncrementCountAction action)
    {
        return state with { ClickCount = state.ClickCount + 1 };
    }

    [ReducerMethod]
    public static CountState2 ReduceIncrementCountAction(CountState2 state, IncrementCountAction action)
    {
        return state with { ClickCount = state.ClickCount + 1 };
    }
}
