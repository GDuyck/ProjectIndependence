using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectIndependence.API.Core.Entities.Customers;
using ProjectIndependence.API.Core.Entities.Inventory;
using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Core.Entities.Sales;
using ProjectIndependence.API.Core.ValueObjects;

namespace ProjectIndependence.API.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        private DbSet<SalesQuotationLine> getSalesQuotationLines;

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductPriceChange> ProductPriceChanges { get; set; }
        public virtual DbSet<Inventory> Inventories { get; set; }
        public DbSet<SalesQuotation> SalesQuotations { get; set; }
        public DbSet<SalesQuotationLine> GetSalesQuotationLines { get => getSalesQuotationLines; set => getSalesQuotationLines = value; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var taxRateConverter = new ValueConverter<TaxRate, int>
                (
                    v => v.Value,
                    v => new TaxRate(v)
                );

            modelBuilder.Entity<Product>()
                .Property(p => p.Tax)
                .HasConversion(taxRateConverter);

            modelBuilder.Entity<ProductPriceChange>()
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey(p => p.ProductId);
        }
    }
}