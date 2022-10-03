using SharedLibrary.Configuration;
using UdemAuthServer.Core.Configuration;
using UdemAuthServer.Core.Services;
using UdemyAuthServer.Service.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<CustomTokenOptions>(builder.Configuration.GetSection("TokenOptions"));
builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Clients"));
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();


app.MapGet("/", () => "Hello World!");

app.Run();
