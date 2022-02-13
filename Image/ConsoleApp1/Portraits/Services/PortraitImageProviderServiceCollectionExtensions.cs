using ConsoleApp1.Portraits.Interfaces;
using ConsoleApp1.Portraits.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp1.Portraits.Services;

public static class PortraitImageProviderServiceCollectionExtensions
{
    public static IServiceCollection AddPortraitImageProvider(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<PortraitImageProviderOptions> options = null)
    {
        services.Configure<PortraitImageProviderOptions>(configuration.GetSection(PortraitImageProviderOptions.CONFIGURE_SECTION_NAME));
        if (options != null)
        {
            services.Configure(options);
        }

        //makig sure atleast a IMemoryCache service exists
        //proably not the best thing to do
        if (!services.Any(x => x.ServiceType == typeof(IMemoryCache)))
        {
            services.AddMemoryCache();
        }

        services.AddScoped<IImageProvider, PortraitImageProvider>();
        return services;
    }
}
