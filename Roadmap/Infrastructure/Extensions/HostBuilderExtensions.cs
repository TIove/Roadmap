using System.Reflection;
using Microsoft.OpenApi.Models;
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

            services.AddSingleton<IStartupFilter, SwaggerStartupFilter>();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                        {Title = $"{Assembly.GetExecutingAssembly().GetName().Name}", Version = "v1"});

                options.CustomSchemaIds(x => x.FullName);

                options.OperationFilter<HeaderOperationFilter>();
            });
        });
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
}