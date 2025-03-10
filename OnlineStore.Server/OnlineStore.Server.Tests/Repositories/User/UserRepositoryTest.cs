using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using OnlineStore.Server.Database.Entities;
using OnlineStore.Server.DTO.Customer;
using OnlineStore.Server.DTO.User;
using OnlineStore.Server.Repositories.User;
using OnlineStore.Server.Tests.Common;

namespace OnlineStore.Server.Tests.Repositories.User
{
    [Collection("DatabaseCollection")]
    public class UserRepositoryTest : IClassFixture<UserDbContextFixture>
    {
        private readonly FakeDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserRepository> _logger;
        private readonly UserDbContextFixture _fixture;

        public UserRepositoryTest(UserDbContextFixture fixture)
        {
            _fixture = fixture;
            _context = _fixture.Context;
            _configuration = new Mock<IConfiguration>().Object;
            _logger = new Mock<ILogger<UserRepository>>().Object;
        }

        [Fact]
        public async Task UpdateUser()
        {
            // Arrange
            var repository = new UserRepository(_context, _configuration, _logger);

            var customer_fail = new UserRequest { Username = _fixture.Username_Unexist, Role = UserRole.Manager };
            var manager_fail = new UserRequest { Username = _fixture.Username_Unexist, Role = UserRole.User };
            var customer_success = new UserRequest { Username = _fixture.CustomerUsername_ForUpdate, Role = UserRole.Manager };
            var manager_success = new UserRequest { Username = _fixture.ManagerUsername_ForUpdate, Role = UserRole.User };

            // Act
            var updateCustomer_Fail_1 = await repository.Update(_fixture.Username_Unexist, customer_fail);
            var updateManager_Fail_2 = await repository.Update(_fixture.Username_Unexist, manager_fail);

            var updateCustomer_Fail_3 = await repository.Update(_fixture.Username_Unexist, customer_success);
            var updateManager_Fail_4 = await repository.Update(_fixture.Username_Unexist, manager_success);

            var updateCustomer_Success = await repository.Update(_fixture.CustomerUsername_ForUpdate, customer_success);
            var updateManager_Success = await repository.Update(_fixture.ManagerUsername_ForUpdate, manager_success);

            // Assert
            Assert.False(updateCustomer_Fail_1);
            Assert.False(updateManager_Fail_2);

            Assert.False(updateCustomer_Fail_3);
            Assert.False(updateManager_Fail_4);

            Assert.True(updateCustomer_Success);
            Assert.True(updateManager_Success);
        }

        [Fact]
        public async Task DeleteUser()
        {
            // Arrange
            var repository = new UserRepository(_context, _configuration, _logger);

            // Act
            var deleteUnexistUser_Fail = await repository.Delete(_fixture.Username_Unexist);

            var deleteCustomer_Success = await repository.Delete(_fixture.CustomerUsername_ForDelete);
            var deleteManager_Success = await repository.Delete(_fixture.ManagerUsername_ForDelete);

            var deleteCustomer_Fail = await repository.Delete(_fixture.CustomerUsername_ForDelete);
            var deleteManager_Fail = await repository.Delete(_fixture.ManagerUsername_ForDelete);

            // Assert
            Assert.False(deleteUnexistUser_Fail);

            Assert.True(deleteCustomer_Success);
            Assert.True(deleteManager_Success);

            Assert.False(deleteCustomer_Fail);
            Assert.False(deleteManager_Fail);
        }

        [Fact]
        public async Task GetPageOfCustomers()
        {
            // Arrange
            var repository = new UserRepository(_context, _configuration, _logger);

            // Act
            var test = await repository.GetAll();

            // Assert
            // there may be range of values because of "register customer/manager" tests
            Assert.InRange(test.TotalCount, _fixture.UsersTotalCount, _fixture.UsersTotalCount + 2);
            Assert.InRange(test.Responses.Count(), _fixture.UsersTotalCount, _fixture.UsersTotalCount + 2);
        }

        [Fact]
        public async Task RegisterManager()
        {
            // Arrange
            var repository = new UserRepository(_context, _configuration, _logger);

            var manager = new UserCredentialsRequest
            {
                Username = "RegisterUser_Manager",
                Password = "myManagerPassword"
            };

            // Act
            var registerManager_Success = await repository.Create(manager);
            var registerManager_Fail = await repository.Create(manager);

            // Assert
            Assert.True(registerManager_Success);
            Assert.False(registerManager_Fail);
        }

        [Fact]
        public async Task RegisterCustomer()
        {
            // Arrange
            var repository = new UserRepository(_context, _configuration, _logger);

            var customer = new CustomerRegisterRequest
            {
                Username = "RegisterUser_Customer2",
                Password = "myCustomerPassword2",
                CustomerInfo = new CustomerBaseRequest
                {
                    Name = _fixture.CustomerName_ForRegister,
                    Code = _fixture.CustomerCode_ForRegister
                }
            };

            // Act
            var registerCustomer_Success = await repository.Create(customer);
            var registerCustomer_Fail = await repository.Create(customer);

            // Assert
            Assert.True(registerCustomer_Success);
            Assert.False(registerCustomer_Fail);
        }
    }
}
