using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Claims;
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

string MyAllowSpecificOrigins = "";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("http://example.com",
                                              "http://www.contoso.com");
                      });
});

//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

//builder.Services.AddAuthorization(o => o.AddPolicy("AdminsOnly",
//                                  b => b.RequireClaim("admin", "true")));

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

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.IncludeFields = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = builder.Environment.ApplicationName,
        Version = "v1"
    });
});



var app = builder.Build();

app.UseFileServer();
app.MapGet("/auth", [Authorize] () => "This endpoint requires authorization.");
app.MapGet("/", () => "This is a GET");
app.MapPost("/", () => "This is a POST");
app.MapPut("/", () => "This is a PUT");
app.MapDelete("/", () => "This is a DELETE");
app.MapMethods("/options-or-head", new[] { "OPTIONS", "HEAD" },
                          () => "This is an options or head request ");
app.MapGet("/hello", () => "Hello named routes")
   .WithName("hi");

app.MapGet("/hello3", () => new { Message = "Hello World" });
//app.MapGet("/api/todoitems/{id}", async (int id, TodoDb db) =>
//         await db.Todos.FindAsync(id).WithName("GetToDoItems").WithTags("TodoGroup" is Todo todo
//         ? Results.Ok(todo)
//         : Results.NotFound()).Produces<Todo>(StatusCodes.Status200OK)
//                              .Produces(StatusCodes.Status404NotFound);
app.MapGet("/routes", (LinkGenerator linker) =>
        $"The link to the hello route is {linker.GetPathByName("hi", values: null)}");

app.MapGet("/hello", () => Results.Json(new { Message = "Hello World" }));
app.MapGet("/text", () => Results.Text("This is some text"));
app.MapGet("/405", () => Results.StatusCode(405));
app.MapGet("/pokemon", async () =>
{
    var proxyClient = new HttpClient();
    var stream = await proxyClient.GetStreamAsync("http://consoto/pokedex.json");
    // Proxy the response as JSON
    return Results.Stream(stream, "application/json");
});
app.MapGet("/old-path", () => Results.Redirect("/new-path"));
app.MapGet("/download", () => Results.File("myfile.text"));


app.MapGet("/users/{userId}/books/{bookId}",
    (int userId, int bookId) => $"The user id is {userId} and book id is {bookId}");
app.MapGet("/posts/{*rest}", (string rest) => $"Routing to {rest}");
app.MapGet("/posts/{slug:regex(^[a-z0-9_-]+$)}", (string slug) => $"Post {slug}");
app.MapGet("/{id}", (int id,
                     int page,
                     [FromHeader(Name = "X-CUSTOM-HEADER")] string customHeader,
                     Service service) => { });

app.MapGet("/todos/{id:int}", (int id) => { }); //db.Todos.Find(id));
app.MapGet("/todos/{text}", (string text) => { });//db.Todos.Where(t => t.Text.Contains(text));
app.MapGet("/posts/{slug:regex(^[a-z0-9_-]+$)}", (string slug) => $"Post {slug}");

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

app.MapGet("/products", (int pageNumber) => $"Requesting page {pageNumber}");
app.MapGet("/products", (int? pageNumber) => $"Requesting page {pageNumber ?? 1}");

string ListProducts(int pageNumber = 1) => $"Requesting page {pageNumber}";

app.MapGet("/products2", ListProducts);
app.MapPost("/products", (Product? product) => product);


app.MapGet("/", (HttpContext context) => context.Response.WriteAsync("Hello World"));
app.MapGet("/", (HttpRequest request, HttpResponse response) =>
    response.WriteAsync($"Hello World {request.Query["name"]}"));
app.MapGet("/", async (CancellationToken cancellationToken) =>
    await Task.Delay(5,cancellationToken));
app.MapGet("/", (ClaimsPrincipal user) => user.Identity.Name);


app.MapGet("/map", (Point point) => $"Point: {point.X}, {point.Y}");

app.MapGet("/products", (PagingData pageData) => $"SortBy:{pageData.SortBy}, " +
       $"SortDirection:{pageData.SortDirection}, CurrentPage:{pageData.CurrentPage}");

app.MapPost("/uploadstream", async (IConfiguration config, HttpRequest request) =>
{
    var filePath = Path.Combine(config["StoredFilesPath"], Path.GetRandomFileName());

    await using var writeStream = File.Create(filePath); //IAsynCDispoable
    await request.BodyReader.CopyToAsync(writeStream);
});

app.MapGet("/html", () => Results.Extensions.Html(@$"<!doctype html>
<html>
    <head><title>miniHTML</title></head>
    <body>
        <h1>Hello World</h1>
        <p>The time on the server is {DateTime.Now:O}</p>
    </body>
</html>"));

app.MapGet("/admin", [Authorize("AdminsOnly")] () =>
                             "The /admin endpoint is for admins only.");

app.MapGet("/admin2", () => "The /admin2 endpoint is for admins only.")
   .RequireAuthorization("AdminsOnly");

app.MapGet("/", () => "This endpoint doesn't require authorization.");
app.MapGet("/Identity/Account/Login", () => "Sign in page at this endpoint.");
app.MapGet("/login", [AllowAnonymous] () => "This endpoint is for all roles.");


app.MapGet("/login2", () => "This endpoint also for all roles.")
   .AllowAnonymous();
app.MapGet("/cors", [EnableCors("MyAllowSpecificOrigins")] () =>
                           "This endpoint allows cross origin requests!");
app.MapGet("/cors2", () => "This endpoint allows cross origin requests!")
                     .RequireCors(MyAllowSpecificOrigins);
app.MapGet("/swag", () => "Hello Swagger!");
app.MapGet("/skipme", () => "Skipping Swagger.")
                    .ExcludeFromDescription();
app.MapGet("/skipme2", () => "Skipping Swagger.")
                    .ExcludeFromDescription();
app.MapGet("/skipme3", () => "Skipping Swagger.")
                    .ExcludeFromDescription();
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
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json",
                                $"{builder.Environment.ApplicationName} v1"));

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

class Product
{
    // These are public fields, not properties.
    public int Id;
    public string? Name;
}
class Service{ }
record Person(string Name, int Age);
class Point
{
    public double X { get; set; }
    public double Y { get; set; }

    public static bool TryParse(string? value, IFormatProvider? provider,
                                out Point? point)
    {
        // Format is "(12.3,10.1)"
        var trimmedValue = value?.TrimStart('(').TrimEnd(')');
        var segments = trimmedValue?.Split(',',
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (segments?.Length == 2
            && double.TryParse(segments[0], out var x)
            && double.TryParse(segments[1], out var y))
        {
            point = new Point { X = x, Y = y };
            return true;
        }

        point = null;
        return false;
    }
}
public class PagingData
{
    public string? SortBy { get; init; }
    public SortDirection SortDirection { get; init; }
    public int CurrentPage { get; init; } = 1;

    public static ValueTask<PagingData?> BindAsync(HttpContext context,
                                                   ParameterInfo parameter)
    {
        const string sortByKey = "sortBy";
        const string sortDirectionKey = "sortDir";
        const string currentPageKey = "page";

        Enum.TryParse<SortDirection>(context.Request.Query[sortDirectionKey],
                                     ignoreCase: true, out var sortDirection);
        int.TryParse(context.Request.Query[currentPageKey], out var page);
        page = page == 0 ? 1 : page;

        var result = new PagingData
        {
            SortBy = context.Request.Query[sortByKey],
            SortDirection = sortDirection,
            CurrentPage = page
        };

        return ValueTask.FromResult<PagingData?>(result);
    }
}

public enum SortDirection
{
    Default,
    Asc,
    Desc
}

class Todo
{

}

static class ResultsExtensions
{
    public static IResult Html(this IResultExtensions resultExtensions, string html)
    {
        ArgumentNullException.ThrowIfNull(resultExtensions);

        return new HtmlResult(html);
    }
}

class HtmlResult : IResult
{
    private readonly string _html;

    public HtmlResult(string html)
    {
        _html = html;
    }

    public Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.ContentType = MediaTypeNames.Text.Html;
        httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(_html);
        return httpContext.Response.WriteAsync(_html);
    }
}