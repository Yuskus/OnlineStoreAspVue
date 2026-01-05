using Moq;
using OnlineStore.Server.Database.Entities;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Users;
using OnlineStore.Server.Repositories.Customers;
using OnlineStore.Server.Repositories.Users;
using OnlineStore.Server.Tests.Services.OrderElement;

namespace OnlineStore.Server.Tests.Services.User
{
    public class UserServiceFixture
    {
        public Guid Guid_Unexists { get; private set; } = Guid.NewGuid();
        public Guid CustomerId_Exists { get; private set; } = Guid.NewGuid();
        public Guid[] UserId_Exists { get; private set; } = [Guid.NewGuid(), Guid.NewGuid()];
        public string Username_Unexists { get; private set; } = "unknown";
        public string[] Username_Exists { get; private set; } = ["username1" , "username2"];
        public string Token_Exists { get; private set; } = "sdfhsyjkmfhnmfulutgkfyhkmdytk";
        public int ResponseTotal { get; private set; }
        public IEnumerable<UserResponse> ResponseList { get; private set; }

        public UserServiceFixture()
        {
            ResponseList =
            [
                new()
                {
                    Id = UserId_Exists[0],
                    Username = Username_Exists[0],
                    Role = UserRole.Manager
                },
                new()
                {
                    Id = UserId_Exists[0],
                    Username = Username_Exists[0],
                    Role = UserRole.User,
                    Customer = new()
                    {
                        Id = CustomerId_Exists,
                        Name = "someName",
                        Code = "3329-2000"
                    }
                }
            ];
            ResponseTotal = ResponseList.Count();
        }

        public Mock<ICustomerRepository> CreateCustomerMockRepository()
        {
            var mockRepository = new Mock<ICustomerRepository>();

            return mockRepository;
        }

        public Mock<IUserRepository> CreateUserMockRepository()
        {
            var mockRepository = new Mock<IUserRepository>();

            // auth

            mockRepository
                .Setup(x => x.Authenticate(
                    It.Is<UserCredentialsRequest>(x => x.Username == Username_Exists[0])))
                .ReturnsAsync(new LoginResponse
                {
                    CustomerId = Guid.NewGuid(),
                    Username = Username_Exists[0],
                    Role = It.IsAny<UserRole>(),
                    Token = Token_Exists
                });

            mockRepository
                .Setup(x => x.Authenticate(
                    It.Is<UserCredentialsRequest>(x => x.Username == Username_Exists[1])))
                .ReturnsAsync(new LoginResponse
                {
                    CustomerId = Guid.NewGuid(),
                    Username = Username_Exists[1],
                    Role = It.IsAny<UserRole>(),
                    Token = Token_Exists
                });

            mockRepository
                .Setup(x => x.Authenticate(
                    It.Is<UserCredentialsRequest>(x => !Username_Exists.Contains(x.Username))))
                .ReturnsAsync(() => null);

            //update

            mockRepository
                .Setup(x => x.Update(
                    Username_Exists[0],
                    It.Is<UserRequest>(x => x.Username != Username_Unexists)))
                .ReturnsAsync(true);

            mockRepository
                .Setup(x => x.Update(
                    Username_Exists[1],
                    It.Is<UserRequest>(x => x.Username != Username_Unexists)))
                .ReturnsAsync(true);

            mockRepository
                .Setup(x => x.Update(
                    It.Is<string>(x => !Username_Exists.Contains(x)),
                    It.IsAny<UserRequest>()))
                .ReturnsAsync(false);

            //delete

            mockRepository
                .SetupSequence(x => x.Delete(Username_Exists[0]))
                .ReturnsAsync(true)
                .ReturnsAsync(false);

            mockRepository
                .SetupSequence(x => x.Delete(Username_Exists[1]))
                .ReturnsAsync(true)
                .ReturnsAsync(false);

            mockRepository
                .Setup(x => x.Delete(Username_Unexists))
                .ReturnsAsync(false);

            // get

            mockRepository
                .Setup(x => x.GetPage(
                    It.Is<PageInfo>(p => p.Number == 1 && p.Size == 12)))
                .ReturnsAsync(() => new(ResponseList, ResponseTotal));

            mockRepository
                .Setup(x => x.GetPage(
                    It.Is<PageInfo>(p => p.Number == 1 && p.Size == 20)))
                .ReturnsAsync(() => new(ResponseList, ResponseTotal));

            mockRepository
                .Setup(x => x.GetPage(
                    It.Is<PageInfo>(p => p.Number == 2 && p.Size == 3)))
                .ReturnsAsync(() => new([], ResponseTotal));

            return mockRepository;
        }
    }

    [CollectionDefinition("UserServiceCollection")]
    public class OrderElementServiceCollection : ICollectionFixture<OrderElementServiceFixture> { }
}
