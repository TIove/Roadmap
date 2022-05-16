using Tiove.Roadmap.Infrastructure.Middlewares;

namespace Tiove.Roadmap.Infrastructure.StartupFilters;

public class TerminalStartupFilter : IStartupFilter
{
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return app =>
        {
            app.Map("/version",
                builder => builder.UseMiddleware<VersionMiddleware>());
            app.Map("/live",
                builder => builder.Run(c => c.Response.WriteAsync("live")));
            app.UseMiddleware<RequestLoggingMiddleware>();
            next(app);
        };
    }
}