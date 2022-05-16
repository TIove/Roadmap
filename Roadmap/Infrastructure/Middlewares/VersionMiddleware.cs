using System.Reflection;

namespace Tiove.Roadmap.Infrastructure.Middlewares;

public class VersionMiddleware
{
    public VersionMiddleware(RequestDelegate next)
    {
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var assemblyName = Assembly.GetExecutingAssembly().GetName();
        string assemblyVersion = assemblyName.Version?.ToString() ?? "No version";
        string assemblyServiceName = assemblyName.Name ?? "No name";

        string resultObject =
            "{" + $"\"version\":\"{assemblyVersion}\", \"serviceName\":\"{assemblyServiceName}\"" + "}";

        await context.Response.WriteAsync(resultObject);
    }
}