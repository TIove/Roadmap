using System.Reflection;
using Microsoft.OpenApi.Models;
using Serilog;
using Tiove.Roadmap.Infrastructure.Filters;
using Tiove.Roadmap.Infrastructure.StartupFilters;
using Tiove.Roadmap.Infrastructure.Swagger;

namespace Tiove.Roadmap.Infrastructure.Extensions;

public static class HostBuilderExtensions
{
    public static IHostBuilder AddInfrastructure(this IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton<IStartupFilter, TerminalStartupFilter>();

#if DEBUG
            services.AddSwagger();
#endif
        });

        builder.AddSerilog();

        return builder;
    }

    public static IHostBuilder AddHttp(this IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddControllers(options => options.Filters.Add<GlobalExceptionFilter>());
        });

        return builder;
    }

    private static void AddSwagger(this IServiceCollection services)
    {
        services.AddSingleton<IStartupFilter, SwaggerStartupFilter>();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1",
                new OpenApiInfo
                    {Title = $"{Assembly.GetExecutingAssembly().GetName().Name}", Version = "v1"});

            options.IncludeXmlComments(@"./bin/Debug/net6.0/Roadmap.Models.Dto.xml");

            options.CustomSchemaIds(x => x.FullName);

            options.OperationFilter<HeaderOperationFilter>();
        });
    }

    private static void AddSerilog(this IHostBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile(
                $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                true)
            .Build();

        Log.Logger = new LoggerConfiguration().ReadFrom
            .Configuration(configuration)
            .Enrich.WithProperty("Service", "UserService")
            .CreateLogger();
        
        builder.UseSerilog();
    }
}