using Microsoft.EntityFrameworkCore;
using RestWithASPNET.Model.Context;
using RestWithASPNET.Business;
using RestWithASPNET.Business.Implementations;
using RestWithASPNET.Repository;
using RestWithASPNET.Repository.Implementations;
using Asp.Versioning;
using System.Runtime.CompilerServices;
using Pomelo.EntityFrameworkCore.MySql;
using MySqlConnector;
using EvolveDb;
using Serilog;
using Microsoft.AspNetCore.Hosting;
using RestWithASPNET.Repository.Base;
using RestWithASPNET.Repository.Repository;
using RestWithASPNET.Hypermedia.Filters;
using RestWithASPNET.Hypermedia.Enricher;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

var app = builder.Build();

// Configure the HTTP request pipelines
app.UseHttpsRedirection();

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
