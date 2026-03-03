using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net.Mime;

namespace Company.Project.Api
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly ProblemDetailsFactory _problemDetailsFactory;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger,
                                     ProblemDetailsFactory problemDetailsFactory)
        {
            _logger = logger;
            _problemDetailsFactory = problemDetailsFactory;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
                                                    Exception exception,
                                                    CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Unhandled exception");

            var problemDetails = _problemDetailsFactory.CreateProblemDetails(
                httpContext,
                statusCode: StatusCodes.Status500InternalServerError,
                detail: "An unexpected error occurred."
            );

            httpContext.Response.ContentType = MediaTypeNames.Application.ProblemJson;
            httpContext.Response.StatusCode = problemDetails.Status ?? 500;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true; // handled
        }
    }
}
