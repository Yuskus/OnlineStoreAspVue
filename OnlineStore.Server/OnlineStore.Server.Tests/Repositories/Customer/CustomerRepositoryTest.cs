using OnlineStore.Server.DTO.Customer;
using OnlineStore.Server.Repositories.Customer;
using OnlineStore.Server.Tests.Common;

namespace OnlineStore.Server.Tests.Repositories.Customer
{
    [Collection("DatabaseCollection")]
    public class CustomerRepositoryTest : IClassFixture<CustomerDbContextFixture>
    {
        private readonly FakeDbContext _context;
        private readonly CustomerDbContextFixture _fixture;

        public CustomerRepositoryTest(CustomerDbContextFixture fixture)
        {
            _fixture = fixture;
            _context = _fixture.Context;
        }

        [Fact]
        public async Task CreateCustomer() // isolated
        {
            // Arrange
            var repository = new CustomerRepository(_context);

            var customers = new[]
            {
                new CustomerBaseRequest() { Name = "Test1", Code = "0001-2000", Address = "some address 1" },
                new CustomerRequest() { Name = "Test2", Code = "0002-2000", Address = null, Discount = 5 }
            };

            // Act
            var test1 = await repository.CreateCustomer(customers[0]); // first customer
            var test2 = await repository.CreateCustomer(customers[1]); // second customer

            var test3 = await repository.CreateCustomer(customers[0]); // equals test1
            var test4 = await repository.CreateCustomer(customers[1]); // equals test2

            // Assert
            Assert.NotNull(test1);
            Assert.NotNull(test2);
            Assert.NotNull(test3);
            Assert.NotNull(test4);

            Assert.NotEqual(test1, Guid.Empty);
            Assert.NotEqual(test2, Guid.Empty);
            Assert.NotEqual(test3, Guid.Empty);
            Assert.NotEqual(test4, Guid.Empty);

            Assert.NotEqual(test1, test2);
            Assert.NotEqual(test3, test4);

            Assert.Equal(test1, test3);
            Assert.Equal(test2, test4);
        }

        [Fact]
        public async Task UpdateCustomer() // isolated
        {
            // Arrange
            var repository = new CustomerRepository(_context);

            var newCustomer = new CustomerRequest()
            {
                Name = "Test4",
                Code = "0004-2005",
                Address = null,
                Discount = _fixture.CustomersTotalCount - 1 // save serial number the same for other tests
            };

            // Act
            var updateCustomer_Success = await repository.UpdateCustomer(_fixture.CustomerId_ForUpdate, newCustomer);
            var updateCustomer_Fail = await repository.UpdateCustomer(_fixture.CustomerId_Unexist, newCustomer);

            // Assert
            Assert.NotEqual(_fixture.CustomerId_ForUpdate, Guid.Empty);
            Assert.NotEqual(_fixture.CustomerId_Unexist, Guid.Empty);

            Assert.True(updateCustomer_Success);
            Assert.False(updateCustomer_Fail);
        }

        [Fact]
        public async Task GetCustomerByCode() // isolated
        {
            // Arrange
            var repository = new CustomerRepository(_context);

            // Act
            var getByCode_Fail = await repository.GetOneByCriteria(new() { Code = _fixture.CustomerCode_Unexists });
            var getByCode_Seccess = await repository.GetOneByCriteria(new() { Code = _fixture.CustomerCode_ForGetByCode });

            // Assert
            Assert.Null(getByCode_Fail);

            Assert.NotNull(getByCode_Seccess);
            Assert.NotEqual(getByCode_Seccess.Id, Guid.Empty);
        }

        [Fact]
        public async Task GetCustomerById() // isolated
        {
            // Arrange
            var repository = new CustomerRepository(_context);

            // Act
            var getById_Fail = await repository.GetOneByCriteria(new() { Id = _fixture.CustomerId_Unexist });
            var getById_Success = await repository.GetOneByCriteria(new() { Id = _fixture.CustomerId_ForGetById });

            // Assert
            Assert.Null(getById_Fail);

            Assert.NotNull(getById_Success);
            Assert.NotEqual(getById_Success.Id, Guid.Empty);
        }

        [Fact]
        public async Task GetPageOfCustomers() // isolated
        {
            // Arrange
            var repository = new CustomerRepository(_context);

            // Act
            var test = await repository.GetAllCustomers();

            // Assert
            // there may be range of values because of "create customer" test
            Assert.InRange(test.TotalCount, _fixture.CustomersTotalCount, _fixture.CustomersTotalCount + 2);
            Assert.InRange(test.Responses.Count(), _fixture.CustomersTotalCount, _fixture.CustomersTotalCount + 2);
        }
    }
}
