using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace Tiove.Roadmap.Infrastructure.Filters;

public class GlobalExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        string exceptionType = context.Exception.GetType().FullName ?? string.Empty;
        string exceptionMessage = context.Exception.Message;
        string stackTrace = context.Exception.StackTrace ?? string.Empty;

        var resultObject = new
        {
            ExceptionType = exceptionType,
            ExceptionMessage = exceptionMessage,
            StackTrace = stackTrace
        };

        Log.Logger.Warning("Exception was thrown {@resultObject}", resultObject);

        var jsonResult = new JsonResult(resultObject)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.Result = jsonResult;
    }
}