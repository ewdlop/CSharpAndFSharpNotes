using MediatR;

namespace BlazorServerApp.Data.CQRS;

public class ListAll
{
    public class WeatherForecastQuery : IRequest<WeatherForecastList>
    {
        public DateTime StartDate { get; set; }
    }

    public class WeatherForecastList
    {
        public IEnumerable<WeatherForecastDTO> Forecasts { get; set; }

        public class WeatherForecastDTO
        {
            public DateTime Date { get; set; }
            public int TemperatureC { get; set; }
            public int TemperatureF { get; set; }
            public string Summary { get; set; }
        }
    }

    public class WeatherForecastModelQueryHandler : IRequestHandler<WeatherForecastQuery, WeatherForecastList>
    {
        private static string[] Summaries = new[] {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm",
            "Balmy", "Hot", "Sweltering", "Scorching" };

        public async Task<WeatherForecastList> Handle(
            WeatherForecastQuery request,
            CancellationToken cancellationToken)
        {
            var rng = new Random();
            var forecasts = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecastList.WeatherForecastDTO
            {
                Date = request.StartDate.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });

            return new WeatherForecastList { Forecasts = forecasts };
        }
    }
}
