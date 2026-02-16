using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using ProjectIndependence.API.Application.Interfaces;

namespace ProjectIndependence.API.Application.Extensions
{
    public static class CqrsServiceCollectionExtensions
    {
        public static IServiceCollection AddCqrs(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (assemblies is null | assemblies.Length == 0)
            {
                assemblies = new[]
                {
                    Assembly.GetExecutingAssembly()
                };
            }

            var allTypes = assemblies
                .SelectMany(a => a.DefinedTypes)
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .ToArray();

            RegisterHandlers(services, allTypes, typeof(ICommandHandler<,>));
            RegisterHandlers(services, allTypes, typeof(IQueryHandler<,>));

            services.AddScoped<IMediator, Mediator>();

            return services;
        }

        private static void RegisterHandlers(IServiceCollection services, TypeInfo[] allTypes, Type openGenericType)
        {
            var registrations = allTypes
                .SelectMany(t => t.ImplementedInterfaces
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == openGenericType)
                    .Select(i => new { Service = i, Implementation = t.AsType() }))
                .ToArray();

            foreach (var registration in registrations)
                services.AddTransient(registration.Service, registration.Implementation);
        }
    }
}