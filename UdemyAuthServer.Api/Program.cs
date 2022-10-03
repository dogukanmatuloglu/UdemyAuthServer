using SharedLibrary.Configuration;
using UdemAuthServer.Core.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<CustomTokenOptions>(builder.Configuration.GetSection("TokenOptions"));
builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Clients"));

var app = builder.Build();


app.MapGet("/", () => "Hello World!");

app.Run();
