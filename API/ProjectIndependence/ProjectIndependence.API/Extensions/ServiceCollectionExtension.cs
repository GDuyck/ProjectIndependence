using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using ProjectIndependence.API.Core.Response;

namespace ProjectIndependence.API.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApiValidation(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();

            services.Configure<ApiBehaviorOptions>(options =>
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                       .Where(kvp => kvp.Value?.Errors.Count > 0)
                       .ToDictionary(
                           kvp => kvp.Key,
                           kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                       );

                    var problemDetails = new ValidationProblemDetails
                    {
                        Title = "One or more validation Errors occurered.",
                        Status = StatusCodes.Status400BadRequest,
                        Errors = errors
                    };

                    var response = new ApiResponse<Object>
                    {
                        Success = false,
                        Error = problemDetails
                    };

                    return new BadRequestObjectResult(response);
                });

            return services;
        }
    }
}
