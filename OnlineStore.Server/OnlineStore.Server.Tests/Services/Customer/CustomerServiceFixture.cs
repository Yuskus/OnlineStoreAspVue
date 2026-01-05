using Moq;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Customers;
using OnlineStore.Server.Repositories.Customers;

namespace OnlineStore.Server.Tests.Services.Customer
{
    public class CustomerServiceFixture
    {
        public Guid Guid_Unexists { get; private set; } = Guid.NewGuid();
        public Guid CustomerId_Exists { get; private set; } = Guid.NewGuid();
        public string CustomerName_Exists { get; private set; } = "Test test";
        public string CustomerCode_Exists { get; private set; } = "9753-2001";
        public int ResponseTotal { get; private set; }
        public IEnumerable<CustomerResponse> ResponseList { get; private set; }

        public CustomerServiceFixture()
        {
            ResponseList = Enumerable.Range(0, 20)
                .Select((x, i) => new CustomerResponse
                {
                    Name = "Test",
                    Code = $"{1000 + i}-2000"
                });
            ResponseTotal = ResponseList.Count();
        }

        public Mock<ICustomerRepository> CreateMockRepository()
        {
            var mockRepository = new Mock<ICustomerRepository>();

            //update

            mockRepository
                .Setup(x => x.Update(
                    CustomerId_Exists,
                    It.Is<CustomerRequest>(c =>
                        c.Name == CustomerName_Exists
                        && c.Code == CustomerCode_Exists)))
                .ReturnsAsync(true);

            mockRepository
                .Setup(x => x.Update(
                    Guid_Unexists,
                    It.IsAny<CustomerRequest>()))
                .ReturnsAsync(false);

            //get

            mockRepository
                .Setup(x => x.GetPage(
                    It.Is<PageInfo>(p => p.Number == 1 && p.Size == 12)))
                .ReturnsAsync(() => new(ResponseList.Take(12), ResponseTotal));

            mockRepository
                .Setup(x => x.GetPage(
                    It.Is<PageInfo>(p => p.Number == 1 && p.Size == 20)))
                .ReturnsAsync(() => new(ResponseList, ResponseTotal));

            mockRepository
                .Setup(x => x.GetPage(
                    It.Is<PageInfo>(p => p.Number == 2 && p.Size == 3)))
                .ReturnsAsync(() => new(ResponseList.Take(3), ResponseTotal));

            return mockRepository;
        }
    }

    [CollectionDefinition("CustomerServiceCollection")]
    public class CustomerServiceCollection : ICollectionFixture<CustomerServiceFixture> { }
}
