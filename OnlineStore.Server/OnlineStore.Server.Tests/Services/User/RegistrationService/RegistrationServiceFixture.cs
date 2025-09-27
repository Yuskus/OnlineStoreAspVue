using OnlineStore.Server.Tests.Services.OrderElement;

namespace OnlineStore.Server.Tests.Services.User.RegistrationService
{
    public class RegistrationServiceFixture
    {
        //public Mock<IUserRepository> CreateMockRepository() { }
    }

    [CollectionDefinition("CustomerRegistrationServiceCollection")]
    public class OrderElementServiceCollection : ICollectionFixture<OrderElementServiceFixture> { }
}
