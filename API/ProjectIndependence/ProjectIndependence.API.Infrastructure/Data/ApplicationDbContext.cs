using Microsoft.EntityFrameworkCore;
using ProjectIndependence.API.Core.Entities.Customers;
using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Core.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndependence.API.Infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SalesQuotation> SalesQuotations { get; set; }
        public DbSet<SalesQuotationLine> GetSalesQuotationLines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
