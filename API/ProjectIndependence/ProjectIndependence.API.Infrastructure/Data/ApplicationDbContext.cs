using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public DbSet<SalesQuotation> SalesQuotations { get; set; }
        public DbSet<SalesQuotationLine> GetSalesQuotationLines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           base.OnModelCreating(modelBuilder);
        }
    }
}
