using ProjectIndependence.API.Core.Entities.Base;
using ProjectIndependence.API.Core.ValueObjects;

namespace ProjectIndependence.API.Core.Entities.Products
{
    public class Product : EntityBase
    {
        // Core info
        public string Name { get; private set; }

        public string ProductCode { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }

        // Classification (can be added later)

        // Pricing
        public decimal RetailPrice { get; set; }

        public decimal CostPrice { get; set; }
        public TaxRate Tax { get; internal set; }

        // Inventory
        public int Stock { get; set; }

        // Auditing
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string CreatedBy { get; set; } // Placeholder, will be updated with person later

        private Product()
        { }

        public Product(Guid id,
            string name,
            string productCode,
            string description,
            bool isActive,
            decimal retailPrice,
            decimal costPrice,
            int tax,
            int stock,
            string createdBy)
        {
            Id = id;
            Name = name;
            ProductCode = productCode;
            Description = description;
            IsActive = isActive;
            RetailPrice = retailPrice;
            CostPrice = costPrice;
            Tax = new TaxRate(tax);
            Stock = stock;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            CreatedBy = createdBy;
        }

        public void ToggleStatus()
        {
            IsActive = !IsActive;
            UpdatedAt = DateTime.Now;
        }

        public void UpdatePrice(decimal newRetailPrice, decimal newCostPrice)
        {
            if(newRetailPrice > 0)
                RetailPrice = newRetailPrice;

            if(newCostPrice > 0)
                CostPrice = newCostPrice;

            UpdatedAt  = DateTime.Now;
        }

        public void UpdateProduct(string productName, string productDescription)
        {
            Name = productName;
            Description = productDescription;
            UpdatedAt = DateTime.Now;
        }
    }
}