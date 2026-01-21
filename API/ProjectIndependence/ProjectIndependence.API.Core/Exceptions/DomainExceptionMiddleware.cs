using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectIndependence.API.Core.Response;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ProjectIndependence.API.Core.Exceptions
{
    public class DomainExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<DomainExceptionMiddleware> _logger;

        public DomainExceptionMiddleware(RequestDelegate next, ILogger<DomainExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, $"A domainexception has occured: {ex.Message}");

                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;


                var problem = new ProblemDetails
                {
                    Title = "Domain error",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                };

                var response = ApiResponse<object>.FromError(problem);

                await WriteJsonAsync(httpContext, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occured");

                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var problem = new ProblemDetails
                {
                    Title = "Internal server error",
                    Detail = "An unexpected error happened. Please try again later",
                    Status = StatusCodes.Status500InternalServerError
                };

                var response = ApiResponse<object>.FromError(problem);

                await WriteJsonAsync(httpContext, response);
            }
        }

        private static async Task WriteJsonAsync(HttpContext httpContext, object response)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}