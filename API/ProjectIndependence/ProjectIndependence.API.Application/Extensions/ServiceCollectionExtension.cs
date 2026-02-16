using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectIndependence.API.Application.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplication
            (this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(ApplicationValidationMarker).Assembly);
            services.AddCqrs(typeof(ProjectIndependence.API.Application.AssemblyReference).Assembly);

            return services;
        }
    }
}