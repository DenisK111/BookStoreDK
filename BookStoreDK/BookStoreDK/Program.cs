using System.Text;
using BookStoreDK.BL.CommandHandlers.BookCommandHandlers;
using BookStoreDK.DL.Repositories.MsSql;
using BookStoreDK.Extensions;
using BookStoreDK.HealthChecks;
using BookStoreDK.HostedServices;
using BookStoreDK.Middleware;
using BookStoreDK.Models.Configurations;
using BookStoreDK.Models.Models.Users;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

var logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .MinimumLevel.Warning()
    .WriteTo.Console(theme: AnsiConsoleTheme.Code)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSerilog(logger);

builder.Services
    .Configure<AdditionalInfoProviderSettings>(
        builder.Configuration.GetSection(nameof(AdditionalInfoProviderSettings)))
    .Configure<KafkaBookConsumerSettings>(
        builder.Configuration.GetSection(nameof(KafkaBookConsumerSettings)))
    .Configure<KafkaPurchaseConsumerSettings>(
        builder.Configuration.GetSection(nameof(KafkaPurchaseConsumerSettings)))
    .Configure<KafkaBookDeliveryConsumerSettings>(
        builder.Configuration.GetSection(nameof(KafkaBookDeliveryConsumerSettings)))
     .Configure<MongoDbConfiguration>(
            builder.Configuration.GetSection(nameof(MongoDbConfiguration)));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    // ADD Jwt Token
    var jwtSecurityScheme = new OpenApiSecurityScheme()
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Put **_ONLY_** your JWT Bearer token in the textbox below",
        Reference = new OpenApiReference()
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        },
    };

    x.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    x.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {jwtSecurityScheme,Array.Empty<string>() }
    });
});

// ADD Jwt Token
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.RegisterRepositories();
builder.Services.RegisterServices();
builder.Services.RegisterKafkaConsumers();
builder.Services.RegisterDataFlowHostedServices();
builder.Services.RegisterCaches();
builder.Services.RegisterHttpProviders();

builder.Services.AddHostedService<KafkaBookCacheHostedService>();

builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));

builder.Services.AddHealthChecks()
    .AddCheck<SqlHealthCheck>("SQL Server")
    .AddCheck<CustomHealthCheck>("Customer Health Check")
    .AddUrlGroup(new Uri("https://google.bg"), name: "Google Service");

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("View", policy => policy.RequireClaim("View"));
    options.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));
});

builder.Services.AddMediatR(typeof(GetAllBooksCommandHandler).Assembly);

builder.Services.AddIdentity<UserInfo, UserRole>()
    .AddUserStore<UserInfoStore>()
    .AddRoleStore<UserRoleStore>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

else
{
    app.UseMiddleware<ErrorHandlerMiddleware>();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.RegisterHealthChecks();

app.Run();
