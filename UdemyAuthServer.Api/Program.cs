using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Configuration;
using UdemAuthServer.Core.Configuration;
using UdemAuthServer.Core.Models;
using UdemAuthServer.Core.Repositories;
using UdemAuthServer.Core.Services;
using UdemAuthServer.Core.UnitOfWork;
using UdemyAuthServer.Data;
using UdemyAuthServer.Data.Repositories;
using UdemyAuthServer.Service.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<CustomTokenOptions>(builder.Configuration.GetSection("TokenOptions"));

builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Clients"));


builder.Services.AddAuthentication(options => { options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).
    AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt => {
        var tokenOptions = builder.Configuration.GetSection("TokenOption").Get<CustomTokenOptions>();
        opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        {
            ValidIssuer = tokenOptions.Issuer,
            ValidAudience = tokenOptions.Audience[0],
            IssuerSigningKey = SignService.GetSymmetricKey(tokenOptions.SecurityKey),
            ValidateIssuerSigningKey = true,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime=true

            ,ClockSkew=TimeSpan.Zero

        };

    
    });



builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IServiceGeneric<,>), typeof(GenericService<,>));
builder.Services.AddDbContext<AppDbContext>(opt =>

opt.UseSqlServer(@"Data Source=.;Initial Catalog=UdemyAuthServer;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));


builder.Services.AddIdentity<UserApp, IdentityRole>(
    opt => {
        opt.User.RequireUniqueEmail = true
        ; opt.Password.RequireNonAlphanumeric=false;
    }
    )
.AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();


var app = builder.Build();


app.MapGet("/", () => "Hello World!");

app.Run();
