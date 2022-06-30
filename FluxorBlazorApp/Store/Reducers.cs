using Fluxor;
using FluxorBlazorApp.Data;

namespace FluxorBlazorApp.Store
{
    public static class Reducers //pure
    {
        [ReducerMethod]
        public static CounterState ReduceIncrementCounterAction(CounterState state, IncrementCounterAction action) =>
          new CounterState(clickCount: state.ClickCount + 1);

        [ReducerMethod]
        public static WeatherState ReduceFetchDataAction(WeatherState state, FetchDataResultAction action) =>
          new WeatherState(isLoading: true, forecasts: action.Forecasts);


        [ReducerMethod(typeof(FetchDataAction<int>))]
        public static WeatherState ReduceFetchDataAction(WeatherState state) =>
          new WeatherState(isLoading: true, forecasts: null);

        [ReducerMethod(typeof(FetchDataAction<string>))]
        public static WeatherState ReduceFetchDataAction2(WeatherState state) =>
            new WeatherState(isLoading: true, forecasts: new List<WeatherForecast>{ });

        //[ReducerMethod]
        //public static WeatherState ReduceFetchDataResultAction(WeatherState state, FetchDataResultAction action) =>
        //  new WeatherState(isLoading: false, forecasts: action.Forecasts);
    }
}