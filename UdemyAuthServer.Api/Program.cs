using SharedLibrary.Configuration;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
builder.Services.Configure<CustomTokenOptions>(builder.Configuration.GetSection("TokenOptions");

app.MapGet("/", () => "Hello World!");

app.Run();
