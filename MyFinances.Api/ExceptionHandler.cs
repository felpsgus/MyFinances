using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyFinances.Domain.Exceptions;

namespace MyFinances.Api;

public class ExceptionHandler : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        var problemDetails = exception switch
        {
            ValidationException validationException => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Type = validationException.GetType().Name,
                Title = "Bad request",
                Detail = validationException.Message,
                Extensions =
                {
                    ["failures"] = validationException.Errors
                }
            },
            DomainException domainException => new ProblemDetails
            {
                Status = domainException.StatusCode,
                Type = domainException.GetType().Name,
                Title = domainException.Title,
                Detail = domainException.Message
            },
            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Type = exception.GetType().Name,
                Title = "Server error",
                Detail = exception.Message,
                Extensions =
                {
                    ["stackTrace"] = exception.StackTrace
                }
            }
        };

        httpContext.Response.StatusCode = (int)problemDetails.Status;
        httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return new ValueTask<bool>(true);
    }
}