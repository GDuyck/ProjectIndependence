using Moq;
using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIndependence.API.Tests.Products
{
    public class ProductRepositoryTest
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly List<Product> _mockProducts;

        public ProductRepositoryTest()
        {
            _mockProducts = new List<Product>
            {
                new Product
                {
                    Name = "Test product 1",
                    Price = 20,
                    Tax = 21
                },
                new Product
                {
                    Name = "Test product 2",
                    Price = 40,
                    Tax = 12
                }
            };

            _productRepositoryMock = new Mock<IProductRepository>();

            _productRepositoryMock.Setup(repo => repo.GetAllAsync()).Returns(() =>  _mockProducts);


        }
    }
}
