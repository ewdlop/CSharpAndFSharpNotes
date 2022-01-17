using BlazorServerApp.Data;
using MediatR;
using Microsoft.AspNetCore.Authentication.Negotiate;
using System.Reflection;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);

//not webhost..host for Asp.net core app okay....01/06/2021
builder.Host.ConfigureAppConfiguration((hostcontext, configuration) =>
{
    configuration.AddAzureAppConfiguration(options =>
    {
        IConfiguration settings = configuration.Build();
        options.Connect(settings["AppConfig"])
               .UseFeatureFlags(featureFlagOptions => {
                   featureFlagOptions.CacheExpirationInterval = TimeSpan.FromMinutes(5);
                });
               //.ConfigureRefresh(refresh =>
               //{
               //    refresh.Register("TestApp:Settings", refreshAll: true)
               //                           .SetCacheExpiration(new TimeSpan(0, 5, 0));
               //});
    });
}).ConfigureServices((hostcontext, service) =>
{
    service.Configure<Settings>(hostcontext.Configuration.GetSection("TestApp:Settings"));
    service.AddFeatureManagement();
});

// Add services to the container.
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
   .AddNegotiate();

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy.
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped(sp => new HttpClient { });
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddAzureAppConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAzureAppConfiguration();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
