using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BuildingBlocks.Exceptions.Handler
{
    public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
        : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError($"Error Message: {exception.Message}, Time of Occurence: {DateTime.UtcNow}");

            (string Details, string Title, int StatusCode) details = exception switch
            {
                InternalServerException =>
                (
                    exception.Message,
                    exception.GetType().Name,
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError
                ),
                ValidationException =>
                (
                exception.Message,
                exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest
                ),
                BadRequestException =>
                (
                exception.Message,
                exception.GetType().Name,
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest
                ),
                NotFoundException =>
                (
                    exception.Message,
                    exception.GetType().Name,
                    httpContext.Response.StatusCode = StatusCodes.Status404NotFound
                ),
                _=>
                (
                exception.Message,
                exception.GetType().Name,
                httpContext.Response.StatusCode= StatusCodes.Status500InternalServerError
                )
            };

            var exceptionDetails = new ProblemDetails
            {
                Title = details.Title,
                Status = details.StatusCode,
                Detail = details.Details,
                Instance = httpContext.Request.Path
            };

            exceptionDetails.Extensions.Add("traceId",httpContext.TraceIdentifier);

            if(exception is ValidationException validationException)
            {
                exceptionDetails.Extensions.Add("ValidationErrors", validationException.Message);
            }

            await httpContext.Response.WriteAsJsonAsync(exceptionDetails,cancellationToken:cancellationToken);

            return true;
        }
    }
}
