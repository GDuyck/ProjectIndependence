using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ProjectIndependence.API.Core.Response;

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