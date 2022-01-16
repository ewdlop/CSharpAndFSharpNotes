using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Reflection;

[assembly: FunctionsStartup(typeof(RealTimeFunctionApp.Startup))]
namespace RealTimeFunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {

        }
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            builder.ConfigurationBuilder.AddUserSecrets(Assembly.GetExecutingAssembly(), true);
        }
    }
}
