using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);
//var builder = WebApplication.CreateBuilder(new WebApplicationOptions
//{
//    Args = args,
//    ApplicationName = typeof(Program).Assembly.FullName,
//    ContentRootPath = Directory.GetCurrentDirectory(),
//    EnvironmentName = Environments.Staging,
//    WebRootPath = "customwwwroot"
//});


//ASPNETCORE_APPLICATIONNAME    --applicationName
//ASPNETCORE_ENVIRONMENT 	    --environment
//ASPNETCORE_CONTENTROOT        --contentRoot

//builder.Host.ConfigureHostOptions(o => o.ShutdownTimeout = TimeSpan.FromSeconds(30));
//builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());


builder.Logging.AddJsonConsole();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<Service>();


//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.ConfigureHttpsDefaults(httpsOptions =>
//    {
//        var certPath = Path.Combine(builder.Environment.ContentRootPath, "cert.pem");
//        var keyPath = Path.Combine(builder.Environment.ContentRootPath, "key.pem");

//        httpsOptions.ServerCertificate = X509Certificate2.CreateFromPemFile(certPath,
//                                         keyPath);
//    });
//});

//builder.Configuration["Kestrel:Certificates:Default:Path"] = "cert.pem";
//builder.Configuration["Kestrel:Certificates:Default:KeyPath"] = "key.pem";
builder.Configuration.AddIniFile("appsettings.ini");

var app = builder.Build();

app.UseFileServer();

app.MapGet("/", () => "This is a GET");
app.MapPost("/", () => "This is a POST");
app.MapPut("/", () => "This is a PUT");
app.MapDelete("/", () => "This is a DELETE");
app.MapMethods("/options-or-head", new[] { "OPTIONS", "HEAD" },
                          () => "This is an options or head request ");
app.MapGet("/hello", () => "Hello named routes")
   .WithName("hi");

app.MapGet("/routes", (LinkGenerator linker) =>
        $"The link to the hello route is {linker.GetPathByName("hi", values: null)}");

app.MapGet("/users/{userId}/books/{bookId}",
    (int userId, int bookId) => $"The user id is {userId} and book id is {bookId}");
app.MapGet("/posts/{*rest}", (string rest) => $"Routing to {rest}");
app.MapGet("/posts/{slug:regex(^[a-z0-9_-]+$)}", (string slug) => $"Post {slug}");
app.MapGet("/{id}", (int id,
                     int page,
                     [FromHeader(Name = "X-CUSTOM-HEADER")] string customHeader,
                     Service service) => { });

app.MapGet("/{id2}", (HttpRequest request) =>
{
    var id = request.RouteValues["id2"];
    var page = request.Query["page"];
    var customHeader = request.Headers["X-CUSTOM-HEADER"];

    // ...
});

app.MapPost("/", async (HttpRequest request) =>
{
    var person = await request.ReadFromJsonAsync<Person>();
});

app.MapGet("/{id3}", ([FromRoute] int id3,
                     [FromQuery(Name = "p")] int page,
                     [FromServices] Service service,
                     [FromHeader(Name = "Content-Type")] string contentType)
                     => { });


app.MapPost("/", (Person person) => { });


Delegate handler = () => "This is a lambda variable";
string LocalFunction() => "This is local function";
HelloHandler helloHandler = new HelloHandler();

app.MapGet("/lambda", handler);
app.MapGet("/local", LocalFunction);
app.MapGet("/helloHandler", helloHandler.Hello);
app.MapGet("/helloHandler2", HelloHandler.Hello2);

app.MapGet("/oops", () => "Oops! An error happened.");
//app.MapGet("/", () =>
//{
//    throw new InvalidOperationException("Oops, the '/' route has thrown an exception.");
//});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/oops");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var port = Environment.GetEnvironmentVariable("PORT") ?? "3000";
app.Run($"http://localhost:{port}");


class HelloHandler
{
    public string Hello()
    {
        return "Hello Instance method";
    }

    public static string Hello2()
    {
        return "Hello Instance method";
    }
}


class Service{ }
record Person(string Name, int Age);