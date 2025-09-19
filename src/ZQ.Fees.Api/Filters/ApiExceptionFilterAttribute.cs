using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ZQ.Fees.Api.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly ILogger<ApiExceptionFilterAttribute> _logger;
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger)
    {
        _logger = logger;
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                // All customise or standard exception types should be added here to handle.
                // or else they will be treated as unhandled exception.
                { typeof(InvalidOperationException), HandleInvalidOperationException },
                { typeof(ValidationException), HandleValidationException }
            };
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);
        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        Type type = context.Exception.GetType();
        if (_exceptionHandlers.ContainsKey(type))
        {
            _exceptionHandlers[type].Invoke(context);
            _logger.LogError(context.Exception, "Exception Message: {exceptionMessage}", context.Exception.Message);

            return;
        }

        if (!context.ModelState.IsValid)
        {
            HandleInvalidModelStateException(context);
            return;
        }

        HandleUnknownException(context);
    }

    private void HandleInvalidOperationException(ExceptionContext context)
    {
        var exception = context.Exception as InvalidOperationException;

        var details = new ProblemDetails()
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "The specified action is not allowed on this resource.",
            Detail = exception.Message
        };

        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;
    }

    private void HandleValidationException(ExceptionContext context)
    {
        var exception = context.Exception as ValidationException;

        var details = new ProblemDetails()
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "Invalid request body.",
            Detail = exception.Message
        };

        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;
    }

    private void HandleInvalidModelStateException(ExceptionContext context)
    {
        var details = new ValidationProblemDetails(context.ModelState)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;

        _logger.LogError(context.Exception, "Exception Message: {exceptionMessage}", context.Exception.Message);
    }

    private void HandleUnknownException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An error occurred while processing your request.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
        context.ExceptionHandled = true;

        _logger.LogError(context.Exception, "Exception Message: {exceptionMessage}", context.Exception.Message);
    }
}
