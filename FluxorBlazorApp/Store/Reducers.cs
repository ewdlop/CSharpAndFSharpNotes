using Fluxor;

namespace FluxorBlazorApp.Store
{
    public static class Reducers //pure
    {
        [ReducerMethod]
        public static CounterState ReduceIncrementCounterAction(CounterState state, IncrementCounterAction action) =>
          new CounterState(clickCount: state.ClickCount + 1);

        [ReducerMethod(typeof(FetchDataAction))]
        public static WeatherState ReduceFetchDataAction(WeatherState state) =>
          new WeatherState(isLoading: true, forecasts: null);

        [ReducerMethod]
        public static WeatherState ReduceFetchDataResultAction(WeatherState state, FetchDataResultAction action) =>
          new WeatherState(isLoading: false, forecasts: action.Forecasts);
    }
}