using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Roadmap.DataProvider.PostgreSql.Ef;
using Tiove.Roadmap.Infrastructure.Mapping;

namespace Tiove.Roadmap.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureDbContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string connStr = Environment.GetEnvironmentVariable("SQLConnectionString");
        if (string.IsNullOrEmpty(connStr))
        {
            connStr = configuration.GetConnectionString("SQLConnectionString");
        }

        services.AddDbContext<RoadmapDbContext>(options =>
        {
            options.UseNpgsql(connStr);
        });
    }

    public static void AddInfrastructureAutoMapper(this IServiceCollection services)
    {
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile<MappingProfile>();
        });

        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);
    }
}