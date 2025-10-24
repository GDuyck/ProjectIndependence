using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Customers;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Products;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Sales;
using ProjectIndependence.API.Core.Interfaces.ServiceInterfaces.Products;
using ProjectIndependence.API.Core.Products.Commands.AdjustProductStock;
using ProjectIndependence.API.Core.Products.Commands.CreateProduct;
using ProjectIndependence.API.Core.Products.Commands.ToggleProductStatus;
using ProjectIndependence.API.Core.Products.Commands.UpdateProduct;
using ProjectIndependence.API.Core.Products.Commands.UpdateProductPrice;
using ProjectIndependence.API.Core.Products.Queries.GetProductById;
using ProjectIndependence.API.Core.Products.Queries.ProductList;
using ProjectIndependence.API.Core.Response;
using ProjectIndependence.API.Core.Services.Products;
using ProjectIndependence.API.Core.Validation;
using ProjectIndependence.API.Core.Validation.Products;
using ProjectIndependence.API.Infrastructure.Data;
using ProjectIndependence.API.Infrastructure.Repositories.Customers;
using ProjectIndependence.API.Infrastructure.Repositories.Products;
using ProjectIndependence.API.Infrastructure.Repositories.Sales;

namespace ProjectIndependence.API.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices
            (this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            string connectionString = configuration.GetConnectionString("DefaultDatabase");

            // Add dbContext
            if (environment.IsEnvironment("Test"))
                connectionString = configuration.GetConnectionString("TestDatabase");

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(connectionString,
                sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

            // Repositories
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ISalesQuotationRepository, SalesQuotationRepository>();
            services.AddScoped<ISalesQuotationLineRepository, SalesQuotationLineRepository>();

            // Services
            services.AddScoped<ProductListQueryHandler>();
            services.AddScoped<GetProductByIdQueryHandler>();
            services.AddScoped<CreateProductCommandHandler>();
            services.AddScoped<UpdateProductCommandHandler>();
            services.AddScoped<UpdateProductPriceCommandHandler>();
            services.AddScoped<AdjustProductStockCommandHandler>();
            services.AddScoped<ProductStatusCommandHandler>();
            services.AddScoped<IProductService, ProductService>();

            return services;
        }

        public static IServiceCollection AddFluentValidationIntegration(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(ApplicationValidationMarker).Assembly);
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