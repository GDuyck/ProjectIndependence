using Microsoft.AspNetCore.Http;
using ProjectIndependence.API.Core.Response;
using System.Text.Json;

namespace ProjectIndependence.API.Core.Exceptions
{
    public class DomainExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public DomainExceptionMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (DomainException ex)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

                var response = ApiResponse<object>.FromValidationErrors(
                    new Dictionary<string, string[]> { { "Domain", new[] { ex.Message } } }
                );

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };

                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response, options));
            }
        }
    }
}