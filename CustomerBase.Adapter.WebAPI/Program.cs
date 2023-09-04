using CustomerBase.Adapter.Adapters.Configurations;
using CustomerBase.Core.ApplicationService.Interfaces;
using CustomerBase.Core.ApplicationService.Services;
using CustomerBase.Core.Domain;
using CustomerBase.Core.Domain.Entities;
using CustomerBase.Core.Domain.Port;
using CustomerBase.Infra.Persistence;
using CustomerBase.Infra.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Data;
using System.Data.SqlClient;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

IdentityModelEventSource.ShowPII = true;
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var key = Encoding.ASCII.GetBytes(Settings.Secret);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    


})
.AddCookie(cfg => cfg.SlidingExpiration = true)
.AddJwtBearer(x =>
{
    x.Audience = "https://localhost:7108";
    x.Authority = "http://localhost:5239";
    //x.Authority = configuration.GetValue<string>("AppSettings:Auth:ServerUrl");
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
        
    };
    x.Configuration = new OpenIdConnectConfiguration();

});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
         .Build());
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Client Control", Version = "v1" });

    // Configurar a inclusão do token no Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization Header",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
//var connectionString = configuration.GetConnectionString("ConnectionStrings");
//builder.Services.AddSingleton<IDbConnection>(provider => new SqlConnection(connectionString));

builder.Services.Configure<ConnectionStringOptions>(configuration.GetSection("ConnectionStrings"));
//builder.Services.AddSingleton<IDbConnection>(provider => new SqlConnection(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<ICustomerBaseService, CustomerBaseService>();



var app = builder.Build();
app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
