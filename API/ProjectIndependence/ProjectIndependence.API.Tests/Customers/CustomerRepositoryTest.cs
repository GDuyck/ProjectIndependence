using Microsoft.Extensions.DependencyInjection;
using ProjectIndependence.API.Core.Entities.Customers;
using ProjectIndependence.API.Core.Entities.Products;
using ProjectIndependence.API.Core.Interfaces.RepositoryInterfaces.Customers;
using ProjectIndependence.API.Infrastructure.Repositories.Customers;
using ProjectIndependence.API.Infrastructure.Repositories.Products;
using ProjectIndependence.API.Tests.Servicebuilder;

namespace ProjectIndependence.API.Tests.Customers
{
    public class CustomerRepositoryTest
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ServiceProvider _serviceProvider;
        private readonly List<Customer> _customers;

        public CustomerRepositoryTest()
        {
            _customers = new List<Customer>
            {
                new Customer
                {
                    Id = Guid.Parse("1134c810-922a-47e2-90d1-ae0ed12901ab"),
                    Name = "Gijs"
                },
                new Customer
                {
                    Id = Guid.Parse("1134c810-922a-47e2-90d1-ae0ed12901bb"),
                    Name = "Lorena"
                }
            };

            _serviceProvider = CreateServiceProvider.CreateProvider<Customer>(_customers);

            _customerRepository = _serviceProvider.GetRequiredService<ICustomerRepository>();
        }

        [Fact]
        public async Task CustomerRepository_GetAllAsync_ReturnsAllCustomersAndIsNotNull()
        {
            // ACT
            var result  = await _customerRepository.GetAllAsync();

            // ASSERT
            Assert.NotNull(result);
            Assert.Equal(_customers.Count, result.Count());
        }

        [Fact]
        public async Task CustomerRepository_GetById_ReturnsProductWithAValidIdAsync()
        {
            // ARRANGE
            var testId = Guid.Parse("1134c810-922a-47e2-90d1-ae0ed12901ab");

            // ACT
            var getByIdResult = await _customerRepository.GetByIdAsync(testId);

            // ASSERT
            Assert.NotNull(getByIdResult);
            Assert.Equal(testId, getByIdResult.Id);
        }

        [Fact]
        public async Task CustomerRepository_GetById_ReturnsNullWithInvalidId()
        {
            // ARRANGE
            var testId = Guid.Parse("1134c810-922a-47e2-90d1-ae0ed129aaab");

            // ACT
            var result = await _customerRepository.GetByIdAsync(testId);

            // ASSERT
            Assert.Null(result);
        }

        [Fact]
        public async Task CustomerRepository_AddAsync_AddProductAndReturnsTheAddedProduct()
        {
            // ARRANGE
            var newCustomer = new Customer
            {
                Id = new Guid(),
                Name = "Customer Number 3",
            };

            // ACT
            var result = await _customerRepository.AddAsync(newCustomer);

            // ASSERT
            Assert.Equal(newCustomer.Name, result.Name);
            Assert.Equal(newCustomer.Id, result.Id);
        }

        [Fact]
        public async Task CustomerRepository_UpdateAsync_ReturnProductWithUpdatedValues()
        {
            // ARRANGE
            string newCustomerName = "Customer formerly known as Gijs";

            // ACT
            var customerToUpdate = await _customerRepository.GetByIdAsync(Guid.Parse("1134c810-922a-47e2-90d1-ae0ed12901ab"));

            customerToUpdate.Name = newCustomerName;

            var result = await _customerRepository.UpdateAsync(customerToUpdate);

            // ASSERT
            Assert.Equal(newCustomerName, result.Name);
            Assert.Equal(customerToUpdate.Id, result.Id);
        }

        [Fact]
        public async Task CustomerRepository_DeleteAsync_ReturnsTrueIfProductExistsAndGetsDeleted()
        {
            // ARRANGE
            var testId = Guid.Parse("1134c810-922a-47e2-90d1-ae0ed12901ab");

            // ACT
            var result = await _customerRepository.DeleteAsync(testId);

            // ASSERT
            Assert.True(result);
        }

        [Fact]
        public async Task CustomerRepository_DeleteAsync_ReturnsFalseIfProductDoesNotExist()
        {
            // ARRANGE
            var testId = Guid.Parse("1134c810-922a-47e2-90d1-ae0ed12901aa");

            // ACT
            var result = await _customerRepository.DeleteAsync(testId);

            // ASSERT
            Assert.False(result);
        }
    }
}