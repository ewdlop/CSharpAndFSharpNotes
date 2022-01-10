using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.DependencyInjection;
using Xunit.DependencyInjection.Logging;

namespace TestProject1
{
    public class Startup
    {
        //public void ConfigureHost(IHostBuilder hostBuilder) =>
        //    hostBuilder.ConfigureWebHost(webHostBuilder => webHostBuilder
        //    .UseTestServer()
        //    .Configure((app) =>
        //    {
        //        app.UseEndpoints(endpoints => endpoints.MapControllers());
        //    })
        //    .ConfigureServices((context, services) =>
        //    {

        //    }));

        public void Configure(ILoggerFactory loggerFactory, ITestOutputHelperAccessor accessor) =>
            loggerFactory.AddProvider(new XunitTestOutputLoggerProvider(accessor));
        public void ConfigureHost(IHostBuilder hostBuilder) =>
            hostBuilder.ConfigureServices((context, services) => {  });
    }
}

