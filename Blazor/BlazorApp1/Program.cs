using BlazorApp1.Areas.Identity.Data;
using BlazorApp1.Data;
using BlazorApp1.Services;
using Fluxor;
using Fluxor.Persist.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BlazorApp1ContextConnection");builder.Services.AddDbContext<BlazorApp1Context>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDefaultIdentity<BlazorApp1User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<BlazorApp1Context>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<ClipboardService>();
var currentAssembly = typeof(Program).Assembly;
builder.Services.AddFluxor(options => 
    options.ScanAssemblies(currentAssembly).UsePersist());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
