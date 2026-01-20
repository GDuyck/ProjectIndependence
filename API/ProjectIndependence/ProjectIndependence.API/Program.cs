using Mapster;
using ProjectIndependence.API.Application.Extensions;
using ProjectIndependence.API.Core.Exceptions;
using ProjectIndependence.API.Extensions;
using ProjectIndependence.API.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80);
    options.ListenAnyIP(443, listenOptions =>
    {
        listenOptions.UseHttps("certs/localhost.pfx", "L1mb0-m@N");
    });
});

builder.Services.AddInfrastructureServices(builder.Configuration, builder.Environment);

// Repositories
builder.Services.AddCqrs(typeof(ProjectIndependence.API.Application.AssemblyReference).Assembly);

builder.Services.AddFluentValidationIntegration();

builder.Services.AddMapster();
MapsterConfig.RegisterMappings();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<DomainExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{ }