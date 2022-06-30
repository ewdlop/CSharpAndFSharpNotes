using Fluxor;
using FluxorBlazorApp.Data;
using FluxorBlazorApp.Store;

namespace FluxorBlazorApp.Pages
{
    public class Effects
    {
        private readonly WeatherForecastService WeatherForecastService;

        public Effects(WeatherForecastService weatherForecastService)
        {
            WeatherForecastService = weatherForecastService;
        }

        [EffectMethod]
        public async Task HandleFetchDataAction(FetchDataAction<string> action, IDispatcher dispatcher)
        {
            var forecasts = await WeatherForecastService.GetForecastAsync(DateTime.Now);
            dispatcher.Dispatch(new FetchDataResultAction(forecasts));
        }
    }
}