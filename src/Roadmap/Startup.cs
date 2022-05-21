using Microsoft.EntityFrameworkCore;
using Roadmap.Data;
using Roadmap.DataProvider.MsSql.Ef;
using Roadmap.Domain.Services;
using Roadmap.Domain.Services.Interfaces;
using Tiove.Roadmap.Infrastructure.Extensions;

namespace Tiove.Roadmap;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IDataProvider, RoadmapDbContext>();

        services.AddMvc().AddNewtonsoftJson();
        
        services.AddInfrastructureDbContext(Configuration);
        services.AddInfrastructureAutoMapper();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        UpdateDatabase(app);
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
    
    private void UpdateDatabase(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope();

        using var context = serviceScope.ServiceProvider.GetService<RoadmapDbContext>();

        context.Database.Migrate();
    }
}