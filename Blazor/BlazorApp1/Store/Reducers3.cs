using Fluxor;

namespace BlazorApp1.Store;

public static class Reducers3
{
    [ReducerMethod]
    public static CountState ReduceIncrementCountAction3(CountState state, IncrementCountAction action)
    {
        return state with { ClickCount = state.ClickCount + 1 };
    }
}
