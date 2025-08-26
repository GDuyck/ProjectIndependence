using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Infrastructure.Data;

namespace ProjectIndependence.API.Tests.Integration
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");

            builder.ConfigureServices(services =>
            {
                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();

                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                db.Database.EnsureDeleted();
                db.Database.Migrate();

                db.Products.AddRange(
                        new Product
                        {
                            Id = Guid.Parse("9ee738a9-2d29-44b0-8d3a-92c8b4f0f622"),
                            Name = "Test product 1",
                            Price = 20,
                            Tax = 21,
                        },
                        new Product
                        {
                            Id = Guid.Parse("1134c810-922a-47e2-90d1-ae0ed12901aa"),
                            Name = "Test product 2",
                            Price = 40,
                            Tax = 12
                        }
                );

                db.SaveChanges();
            });
        }
    }
}