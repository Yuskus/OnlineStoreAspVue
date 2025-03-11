using Moq;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Item;
using OnlineStore.Server.DTO.Order;
using OnlineStore.Server.Repositories.Order;

namespace OnlineStore.Server.Tests.Services.Order
{
    public class OrderServiceFixture
    {
        public DateOnly Today { get; private set; } = DateOnly.FromDateTime(DateTime.Now);
        public Guid Guid_Unexists { get; private set; } = Guid.NewGuid();
        public Guid CustomerId_Exists { get; private set; } = Guid.NewGuid();
        public Guid OrderId_Exists { get; private set; } = Guid.NewGuid();

        public Mock<IOrderRepository> CreateMockRepository()
        {
            var mockRepository = new Mock<IOrderRepository>();

            //create

            mockRepository.Setup(x => x.Create(It.Is<OrderRequest>(x => x.CustomerId == CustomerId_Exists))).ReturnsAsync(Guid.NewGuid());

            mockRepository.Setup(x => x.Create(It.Is<OrderRequest>(x => x.CustomerId == Guid_Unexists))).ReturnsAsync(() => null);

            //update

            mockRepository.Setup(x => x.Update(CustomerId_Exists, It.IsAny<OrderRequest>())).ReturnsAsync(true);

            mockRepository.Setup(x => x.Update(Guid_Unexists, It.IsAny<OrderRequest>())).ReturnsAsync(false);

            mockRepository.Setup(x => x.Update(OrderId_Exists, It.IsAny<OrderRequest>())).ReturnsAsync(true);

            //delete

            mockRepository.SetupSequence(x => x.Delete(CustomerId_Exists)).ReturnsAsync(true).ReturnsAsync(false);

            mockRepository.Setup(x => x.Delete(Guid_Unexists)).ReturnsAsync(false);

            //get

            mockRepository.Setup(x => x.GetOneByCriteria(It.Is<OrderFilterCriteria>(x => x.Id == OrderId_Exists
                                                                                      || x.CustomerId == CustomerId_Exists
                                                                                      || (x.Id == null && x.CustomerId == null && x.OrderStatus == null && x.OrderNumber == null))))
                          .ReturnsAsync(() => new OrderResponse
                          {
                              Id = OrderId_Exists,
                              CustomerId = CustomerId_Exists,
                              OrderDate = Today,
                              OrderStatus = "basket"
                          });

            mockRepository.Setup(x => x.GetOneByCriteria(It.Is<OrderFilterCriteria>(x => x.CustomerId == Guid_Unexists)))
                          .ReturnsAsync(() => null);

            mockRepository.Setup(x => x.GetAll())
                          .ReturnsAsync(() => new ResponseList<OrderResponse>()
                          {
                              TotalCount = 1,
                              Responses = [new OrderResponse()
                              {
                                  Id = OrderId_Exists,
                                  CustomerId = CustomerId_Exists,
                                  OrderDate = Today,
                                  CustomerName = "Test",
                                  OrderNumber = 1,
                                  OrderStatus = "new"
                              }]
                          });

            mockRepository.Setup(x => x.GetAllByCriteria(It.Is<OrderFilterCriteria>(x => x.Id == OrderId_Exists
                                                                                      || x.CustomerId == CustomerId_Exists
                                                                                      || (x.Id == null && x.CustomerId == null && x.OrderStatus == null && x.OrderNumber == null))))
                          .ReturnsAsync(() => new ResponseList<OrderResponse>
                          {
                              TotalCount = 1,
                              Responses = [new OrderResponse()
                              {
                                  Id = OrderId_Exists,
                                  CustomerId = CustomerId_Exists,
                                  OrderDate = Today,
                                  CustomerName = "Test",
                                  OrderNumber = 1,
                                  OrderStatus = "new"
                              }]
                          });

            return mockRepository;
        }
    }

    [CollectionDefinition("OrderServiceCollection")]
    public class OrderServiceCollection : ICollectionFixture<OrderServiceFixture> { }
}
