using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VendingMachineSystem.Domain.Exceptions;

namespace VendingMachineSystem.Api.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An exception occurred: {Message}", exception.Message);

        var (statusCode, title) = exception switch
        {
            // Machine-level
            ShelfNotFoundException => (StatusCodes.Status404NotFound, exception.Message),
            MaxShelvesExceededException => (StatusCodes.Status409Conflict, exception.Message),

            // Shelf-level
            ShelfCapacityExceededException => (StatusCodes.Status409Conflict, exception.Message),
            InvalidProductTypeException => (StatusCodes.Status400BadRequest, exception.Message),
            ShelfOutOfStockException => (StatusCodes.Status400BadRequest, exception.Message),

            // Generic domain
            DomainException => (StatusCodes.Status400BadRequest, exception.Message),

            // Framework-level
            ArgumentException => (StatusCodes.Status400BadRequest, exception.Message),
            InvalidOperationException => (StatusCodes.Status400BadRequest, exception.Message),

            // Unexpected
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = exception.Message,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["errorCode"] = exception.GetType().Name;

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
