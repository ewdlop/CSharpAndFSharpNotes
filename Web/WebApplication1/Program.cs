using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
using Azure.Identity;
using Microsoft.Extensions.Azure;
using WebApplication1.Provider;

var builder = WebApplication.CreateBuilder(args);

//var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
//builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());
//builder.Configuration.AddAzureAppConfiguration("");
builder.Configuration.AddApplicationInsightsSettings();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton(typeof(ITelemetryChannel),
                        new ServerTelemetryChannel() { StorageFolder = "/tmp/myfolder" });
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["Test:blob"], preferMsi: true);
    clientBuilder.AddQueueServiceClient(builder.Configuration["Test:queue"], preferMsi: true);
});

builder.Services.AddHttpClient();

builder.Services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
builder.Services.AddScoped<AzureBlobStorageServiceProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.Use(async (context, next) =>
//{
//    string path = context.Request.Path.Value;
//    if (path != null && !path.ToLower().Contains("/api"))
//    {
//        // XSRF-TOKEN used by angular in the $http if provided
//        var tokens = Antiforgery.GetAndStoreTokens(context);
//        context.Response.Cookies.Append("XSRF-TOKEN",
//          tokens.RequestToken, new CookieOptions
//          {
//              HttpOnly = false,
//              Secure = true
//          }
//        );
//    }
 
//    await next();
//});


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
