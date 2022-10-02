using SharedLibrary.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<CustomTokenOptions>(builder.Configuration.GetSection("TokenOptions"));
var app = builder.Build();


app.MapGet("/", () => "Hello World!");

app.Run();
