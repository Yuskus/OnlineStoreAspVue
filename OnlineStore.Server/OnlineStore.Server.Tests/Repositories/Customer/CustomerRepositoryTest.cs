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
        public async Task CreateCustomer()
        {
            // Arrange
            var repository = new CustomerRepository(_context);

            var baseCustomerRequest = new CustomerBaseRequest()
            {
                Name = "Test1",
                Code = "0001-2000",
                Address = "some address 1"
            };

            var usualCustomerRequest = new CustomerRequest()
            {
                Name = "Test2",
                Code = "0002-2000",
                Address = null,
                Discount = 5
            };

            // Act
            var test1 = await repository.Create(baseCustomerRequest); // first customer
            var test2 = await repository.Create(usualCustomerRequest); // second customer

            var test3 = await repository.Create(baseCustomerRequest); // equals test1
            var test4 = await repository.Create(usualCustomerRequest); // equals test2

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
        public async Task UpdateCustomer()
        {
            // Arrange
            var repository = new CustomerRepository(_context);

            var newCustomer = new CustomerRequest()
            {
                Name = "Test4",
                Code = "0004-2005"
            };

            // Act
            var updateCustomer_Success = await repository.Update(_fixture.CustomerId_ForUpdate, newCustomer);
            var updateCustomer_Fail = await repository.Update(_fixture.CustomerId_Unexists, newCustomer);

            // Assert
            Assert.True(updateCustomer_Success);
            Assert.False(updateCustomer_Fail);
        }

        [Fact]
        public async Task GetCustomerByCriteria()
        {
            // Arrange
            var repository = new CustomerRepository(_context);

            // Act
            var getByCode_Fail = await repository.GetOneByCriteria(new() { Code = _fixture.CustomerCode_Unexists });
            var getByCode_Success = await repository.GetOneByCriteria(new() { Code = _fixture.CustomerCode_ForGetByCode });

            var getById_Fail = await repository.GetOneByCriteria(new() { Id = _fixture.CustomerId_Unexists });
            var getById_Success = await repository.GetOneByCriteria(new() { Id = _fixture.CustomerId_ForGetById });

            // Assert
            Assert.Null(getByCode_Fail);

            Assert.NotNull(getByCode_Success);
            Assert.NotEqual(getByCode_Success.Id, Guid.Empty);

            Assert.Null(getById_Fail);

            Assert.NotNull(getById_Success);
            Assert.NotEqual(getById_Success.Id, Guid.Empty);
        }

        [Fact]
        public async Task GetPageOfCustomers()
        {
            // Arrange
            var repository = new CustomerRepository(_context);

            // Act
            var test = await repository.GetAll();

            // Assert
            // there may be range of values because of "create customer" test
            Assert.InRange(test.TotalCount, _fixture.CustomersTotalCount, _fixture.CustomersTotalCount + 2);
            Assert.InRange(test.Responses.Count(), _fixture.CustomersTotalCount, _fixture.CustomersTotalCount + 2);
        }
    }
}
