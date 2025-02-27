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
        /* Authenticate ??? */

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
        public async Task UpdateUser() // isolated
        {
            // Arrange
            var repository = new UserRepository(_context, _configuration, _logger);

            var customer = new UserRequest { Username = _fixture.CustomerUsername_ForUpdate, Role = UserRole.Manager };
            var manager = new UserRequest { Username = _fixture.ManagerUsername_ForUpdate, Role = UserRole.User };

            // Act
            var updateUnexistCustomer_Fail = await repository.UpdateUser(_fixture.Username_Unexist, customer);
            var updateUnexistManager_Fail = await repository.UpdateUser(_fixture.Username_Unexist, manager);

            var updateCustomer_Success = await repository.UpdateUser(_fixture.CustomerUsername_ForUpdate, customer);
            var updateManager_Success = await repository.UpdateUser(_fixture.ManagerUsername_ForUpdate, manager);

            // Assert
            Assert.False(updateUnexistCustomer_Fail);
            Assert.False(updateUnexistManager_Fail);

            Assert.True(updateCustomer_Success);
            Assert.True(updateManager_Success);
        }

        [Fact]
        public async Task DeleteUser() // isolated
        {
            // Arrange
            var repository = new UserRepository(_context, _configuration, _logger);

            // Act
            var deleteUnexistUser_Fail = await repository.DeleteUser(_fixture.Username_Unexist);

            var deleteCustomer_Success = await repository.DeleteUser(_fixture.CustomerUsername_ForDelete);
            var deleteManager_Success = await repository.DeleteUser(_fixture.ManagerUsername_ForDelete);

            var deleteCustomer_Fail = await repository.DeleteUser(_fixture.CustomerUsername_ForDelete);
            var deleteManager_Fail = await repository.DeleteUser(_fixture.ManagerUsername_ForDelete);

            // Assert
            Assert.False(deleteUnexistUser_Fail);

            Assert.True(deleteCustomer_Success);
            Assert.True(deleteManager_Success);

            Assert.False(deleteCustomer_Fail);
            Assert.False(deleteManager_Fail);
        }

        [Fact]
        public async Task GetPageOfCustomers() // isolated
        {
            // Arrange
            var repository = new UserRepository(_context, _configuration, _logger);

            // Act
            var test1 = await repository.GetPageOfUsersInfo(1, 5);
            var test2 = await repository.GetPageOfUsersInfo(2, 5);
            var test3 = await repository.GetPageOfUsersInfo(3, 3);
            var test4 = await repository.GetPageOfUsersInfo(1, 20);
            var test5 = await repository.GetPageOfUsersInfo(5, 1);

            // Assert
            // there may be range of values because of "register customer/manager" tests
            Assert.InRange(test1.TotalCount, _fixture.UsersTotalCount, _fixture.UsersTotalCount + 2);
            Assert.InRange(test2.TotalCount, _fixture.UsersTotalCount, _fixture.UsersTotalCount + 2);
            Assert.InRange(test3.TotalCount, _fixture.UsersTotalCount, _fixture.UsersTotalCount + 2);
            Assert.InRange(test4.TotalCount, _fixture.UsersTotalCount, _fixture.UsersTotalCount + 2);
            Assert.InRange(test5.TotalCount, _fixture.UsersTotalCount, _fixture.UsersTotalCount + 2);

            Assert.Equal(5, test1.Responses.Count());
            Assert.Equal(5, test2.Responses.Count());
            Assert.Equal(3, test3.Responses.Count());
            Assert.Equal(20, test4.Responses.Count());
            Assert.Single(test5.Responses);
        }

        [Fact]
        public async Task RegisterManager()
        {
            // Arrange
            var repository = new UserRepository(_context, _configuration, _logger);

            var manager = new ManagerRegisterRequest
            {
                Username = "RegisterUser_Manager",
                Password = "myManagerPassword"
            };

            // Act
            var registerManager_Success = await repository.RegisterManager(manager);
            var registerManager_Fail = await repository.RegisterManager(manager);

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
            var registerCustomer_Success = await repository.RegisterUser(_fixture.CustomerId_ForRegister, customer);
            var registerCustomer_Fail = await repository.RegisterUser(_fixture.CustomerId_ForRegister, customer);

            // Assert
            Assert.True(registerCustomer_Success);
            Assert.False(registerCustomer_Fail);
        }
    }
}
