
using BookStoreDK.Extensions;
using BookStoreDK.HealthChecks;
using FluentValidation;
using FluentValidation.AspNetCore;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

var logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .MinimumLevel.Warning()
    .WriteTo.Console(theme: AnsiConsoleTheme.Code)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSerilog(logger);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterRepositories();
builder.Services.RegisterServices()
    .AddAutoMapper(typeof(Program));

builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));

builder.Services.AddHealthChecks()
    .AddCheck<SqlHealthCheck>("SQL Server")
    .AddCheck<CustomHealthCheck>("Customer Health Check")
    .AddUrlGroup(new Uri("https://google.bg"), name: "Google Service");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.RegisterHealthChecks();

app.Run();
