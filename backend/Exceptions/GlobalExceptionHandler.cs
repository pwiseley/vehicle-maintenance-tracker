using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace VehicleMaintenanceTracker.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not (NotFoundException or AlreadyExistsException))
        {
            _logger.LogError(exception, "Unhandled exception");
        }

        ProblemDetails problemDetails = exception switch
        {
            NotFoundException e => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Resource not found",
                Detail = e.Message
            },
            AlreadyExistsException e => new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Resource already exists",
                Detail = e.Message
            },
            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal server error"
            }
        };

        context.Response.StatusCode = problemDetails.Status!.Value;
        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}