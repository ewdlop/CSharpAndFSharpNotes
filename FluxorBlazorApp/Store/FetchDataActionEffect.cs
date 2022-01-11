using Fluxor;
using FluxorBlazorApp.Data;
using FluxorBlazorApp.Store;

namespace FluxorBlazorApp.Pages
{

    //public class FetchDataActionEffect : Effect<FetchDataAction>
    //{
    //    private readonly WeatherForecastService WeatherForecastService;

    //    public FetchDataActionEffect(WeatherForecastService weatherForecastService)
    //    {
    //        WeatherForecastService = weatherForecastService;
    //    }

    //    public override async Task HandleAsync(FetchDataAction action, IDispatcher dispatcher)
    //    {
    //        var forecasts = await WeatherForecastService.GetForecastAsync(DateTime.Now);
    //        dispatcher.Dispatch(new FetchDataResultAction(forecasts));
    //    }
    //}
}