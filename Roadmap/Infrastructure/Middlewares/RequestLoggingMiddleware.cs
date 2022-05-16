namespace Tiove.Roadmap.Infrastructure.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        LogRequest(context);

        await _next(context);

        LogResponse(context);
    }

    private void LogRequest(HttpContext context)
    {
        if (string.Equals(context.Response.ContentType, "application/grpc",
                StringComparison.CurrentCultureIgnoreCase))
        {
            return;
        }

        try
        {
            context.Request.EnableBuffering();

            string fullRequestPath = context.Request.PathBase + context.Request.Path;

            _logger.LogInformation("Request logged");
            _logger.LogInformation("Headers: {headers}", context.Request.Headers);
            _logger.LogInformation("Route: {route}", fullRequestPath);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not log request body");
        }
    }

    private void LogResponse(HttpContext context)
    {
        if (string.Equals(context.Response.ContentType, "application/grpc",
                StringComparison.CurrentCultureIgnoreCase))
        {
            return;
        }

        try
        {
            _logger.LogInformation("Response logged");
            _logger.LogInformation("Headers: {headers}", context.Response.Headers);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not log response body");
        }
    }
}