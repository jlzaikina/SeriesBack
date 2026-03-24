using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Ticket_vkr.Infrastructure;

using Ticket_vkr.Infrastructure.Adapters.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Ticket_vkr.Infrastructure;

using Ticket_vkr.Infrastructure.Adapters.Postgres;
using Ticket_vkr.UI.Application.Handlers;
using Ticket_vkr.UI.Ports;

namespace Ticket_vkr.UI;

public class Startup
{
    public Startup()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
            .AddEnvironmentVariables();
        var configuration = builder.Build();
        Configuration = configuration;
    }


    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {

        // Configuration
        var connectionString = Configuration["CONNECTION_STRING"];

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseExceptionProcessor();
            options.UseNpgsql(connectionString);
        }, ServiceLifetime.Scoped);

        //Регистрация сервисов
        services.AddTransient<IHandler, Handler>();
        services.AddTransient<IRepository, UserPostgresRepository>();

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        services.AddControllers();

        //Swagger
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Сервис сериалов",
                Description = "Сервис сериалов (ASP.NET Core 8.0)",
            });
        });
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddHealthChecks();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin() // Разрешить запросы с любого источника
                    .AllowAnyMethod() // Разрешить все HTTP-методы (GET, POST и т.д.)
                    .AllowAnyHeader(); // Разрешить все заголовки
            });
        });

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCors("AllowAll");
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Сервис сериалов");
            options.RoutePrefix = string.Empty;
        });
        app.UseHealthChecks("/health");
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
