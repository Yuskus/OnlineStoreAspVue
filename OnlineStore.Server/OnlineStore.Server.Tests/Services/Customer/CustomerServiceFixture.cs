using Moq;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Customer;
using OnlineStore.Server.Repositories.Customer;

namespace OnlineStore.Server.Tests.Services.Customer
{
    public class CustomerServiceFixture
    {
        public Guid Guid_Unexists { get; private set; } = Guid.NewGuid();
        public Guid CustomerId_Exists { get; private set; } = Guid.NewGuid();
        public string CustomerName_Exists { get; private set; } = "Test test";
        public string CustomerCode_Exists { get; private set; } = "9753-2001";

        public Mock<ICustomerRepository> CreateMockRepository()
        {
            var mockRepository = new Mock<ICustomerRepository>();

            //update

            mockRepository.Setup(x => x.Update(CustomerId_Exists, It.Is<CustomerRequest>(c => c.Name == CustomerName_Exists
                                                                                           && c.Code == CustomerCode_Exists)))
                          .ReturnsAsync(true);

            mockRepository.Setup(x => x.Update(Guid_Unexists, It.IsAny<CustomerRequest>())).ReturnsAsync(false);

            //get

            mockRepository.Setup(x => x.GetOneByCriteria(It.Is<CustomerFilterCriteria>(c => c.Id == CustomerId_Exists)))
                          .ReturnsAsync(new CustomerResponse { Id = CustomerId_Exists, Code = "0000-2000", Name = CustomerName_Exists });

            mockRepository.Setup(x => x.GetOneByCriteria(It.Is<CustomerFilterCriteria>(c => c.Code == CustomerCode_Exists)))
                          .ReturnsAsync(new CustomerResponse { Code = CustomerCode_Exists, Name = CustomerName_Exists });

            mockRepository.Setup(x => x.GetAll())
                          .ReturnsAsync(() => new ResponseList<CustomerResponse>()
                          {
                              Responses = Enumerable.Range(0, 20).Select((x, i) => new CustomerResponse { Name = "Test", Code = $"{1000 + i}-2000" }).ToList(),
                              TotalCount = 20
                          });

            return mockRepository;
        }
    }

    [CollectionDefinition("CustomerServiceCollection")]
    public class CustomerServiceCollection : ICollectionFixture<CustomerServiceFixture> { }
}
