using Asp.Versioning;
using EvolveDb;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using RestWithASPNET.Business;
using RestWithASPNET.Business.Implementations;
using RestWithASPNET.Configurations;
using RestWithASPNET.Hypermedia.Enricher;
using RestWithASPNET.Hypermedia.Filters;
using RestWithASPNET.Model.Context;
using RestWithASPNET.Repository;
using RestWithASPNET.Repository.Base;
using RestWithASPNET.Repository.Repository;
using RestWithASPNET.Sevices;
using RestWithASPNET.Sevices.Implementations;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

#region CONFIGURACOES PARA AUTHENTICATION

var tokenConfigurations = new TokenConfiguration();
new ConfigureFromConfigurationOptions<TokenConfiguration>(
        Configuration.GetSection("TokenConfigurations")).Configure(tokenConfigurations);

builder.Services.AddSingleton(tokenConfigurations);

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = tokenConfigurations.Issuer,
        ValidAudience = tokenConfigurations.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigurations.Secret))
    };
});

builder.Services.AddAuthorization(auth =>
{
    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser().Build());
});

#endregion





//CORS
builder.Services.AddCors(options => options.AddDefaultPolicy(builder =>
{
    builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
}));

// Add services to the container.
builder.Services.AddControllers();

//configuracao de conexao MYSQL
var connection = builder.Configuration["MySQLConnection:MySQLConnectionString"];

builder.Services.AddDbContext<MySQLContext>(options => options.UseMySql(
    connection, 
    new MySqlServerVersion(new Version(8, 0, 29)))
);

//MIGRATIONS
if (builder.Environment.IsDevelopment())
{
    MigrateDatabase(connection);
}

//HATEOAS
var filterOptions = new HyperMediaFiltersOptions();
filterOptions.ContentResponseEnricherList.Add(new PersonEnricher());
builder.Services.AddSingleton(filterOptions);


//builder.Services.AddApiVersioning();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Rest Api with ASP .NET",
        Version = "v1",
        Description = "Developed in course",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Emerson Sampaio",
            Url = new Uri("https://github.com/emersonsmp")
        }
    });
});

//injeçao de dependencia
builder.Services.AddScoped<IPersonBusiness, PersonBusinessImplementation>();
builder.Services.AddScoped<IBookBusiness, BookBusinessImplementation>();
builder.Services.AddScoped<ILoginBusiness, LoginBusinessImplementation>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

var app = builder.Build();

// Configure the HTTP request pipelines
app.UseHttpsRedirection();

//CORS
app.UseCors();

//swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rest Api with ASP .NET");
});
var option = new RewriteOptions();
option.AddRedirect("^$","swagger");
app.UseRewriter(option);

app.UseAuthorization();

app.MapControllers();

//HATEOAS
app.MapControllerRoute("DefaultApi", "{controller=values}/{id?}");

app.Run();

void MigrateDatabase(string connection)
{
    try
    {
        var evolveConnection = new MySqlConnection(connection);
        var evolve = new Evolve(evolveConnection, Log.Information)
        {
            Locations = new List<string> { "DB/Migrations", "DB/DataSet"},
            IsEraseDisabled = true,
        };

        evolve.Migrate();
    }
    catch (Exception e)
    {
        Log.Error("Database Migration failed", e);
        throw;
    }
}
