using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Tiove.Roadmap.Infrastructure.Filters;

public class GlobalExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        string exceptionType = context.Exception.GetType().FullName ?? string.Empty;
        string stackTrace = context.Exception.StackTrace ?? string.Empty;

        var resultObject = new
        {
            ExceptionType = exceptionType,
            StackTrace = stackTrace
        };

        var jsonResult = new JsonResult(resultObject)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.Result = jsonResult;
    }
}