namespace OnlineStore.Server.Tests.Services.Customer
{
    public class CustomerServiceFixture
    {
        public Guid CustomerId_Exists { get; private set; } = Guid.NewGuid();
        public Guid Guid_Unexists { get; private set; } = Guid.NewGuid();
        public string CustomerCode_Exists { get; private set; } = "9753-2001";
    }

    [CollectionDefinition("CustomerServiceCollection")]
    public class CustomerServiceCollection : ICollectionFixture<CustomerServiceFixture> { }
}
