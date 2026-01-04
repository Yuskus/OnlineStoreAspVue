using Moq;
using OnlineStore.Server.Constants.Orders;
using OnlineStore.Server.Database.Entities;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Orders;
using OnlineStore.Server.Repositories.Orders;

namespace OnlineStore.Server.Tests.Services.Order
{
    public class OrderServiceFixture
    {
        public DateOnly Today { get; private set; } = DateOnly.FromDateTime(DateTime.Now);
        public Guid Guid_Unexists { get; private set; } = Guid.NewGuid();
        public Guid CustomerId_Exists { get; private set; } = Guid.NewGuid();
        public Guid OrderId_Exists { get; private set; } = Guid.NewGuid();
        public int ResponseTotal { get; private set; }
        public IEnumerable<OrderResponse> ResponseList { get; private set; }
        public OrderResponse Basket {  get; private set; }

        public OrderServiceFixture()
        {
            ResponseList =
            [
                new OrderResponse()
                {
                    Id = OrderId_Exists,
                    CustomerId = CustomerId_Exists,
                    OrderDate = Today,
                    CustomerName = "Test",
                    OrderNumber = 1,
                    OrderStatus = OrderStatuses.New
                }
            ];
            ResponseTotal = ResponseList.Count();
            Basket = new OrderResponse
            {
                Id = OrderId_Exists,
                CustomerId = CustomerId_Exists,
                OrderDate = Today,
                OrderStatus = OrderStatuses.Basket
            };
        }

        public Mock<IOrderRepository> CreateMockRepository()
        {
            var mockRepository = new Mock<IOrderRepository>();

            //create

            mockRepository
                .Setup(x => x.Create(
                    It.Is<OrderRequest>(x => x.CustomerId == CustomerId_Exists)))
                .ReturnsAsync(Guid.NewGuid());

            mockRepository
                .Setup(x => x.Create(
                    It.Is<OrderRequest>(x => x.CustomerId == Guid_Unexists)))
                .ReturnsAsync(() => null);

            //update

            mockRepository
                .Setup(x => x.Update(
                    OrderId_Exists,
                    It.IsAny<OrderRequest>()))
                .ReturnsAsync(true);

            mockRepository
                .Setup(x => x.Update(
                    Guid_Unexists,
                    It.IsAny<OrderRequest>()))
                .ReturnsAsync(false);

            //delete

            mockRepository
                .SetupSequence(x => x.Delete(CustomerId_Exists))
                .ReturnsAsync(true)
                .ReturnsAsync(false);

            mockRepository
                .Setup(x => x.Delete(Guid_Unexists))
                .ReturnsAsync(false);

            //get

            mockRepository
                .Setup(x => x.GetOneByCriteria(
                    It.Is<OrderFilterCriteria>(x => x.Id == OrderId_Exists
                        || x.CustomerId == CustomerId_Exists
                        || (x.Id == null && x.CustomerId == null && x.OrderStatus == null && x.OrderNumber == null))))
                .ReturnsAsync(() => Basket);

            mockRepository
                .Setup(x => x.GetOneByCriteria(
                    It.Is<OrderFilterCriteria>(x => x.CustomerId == Guid_Unexists)))
                .ReturnsAsync(() => null);

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

            mockRepository
                .Setup(x => x.GetPageByCriteria(
                    It.Is<OrderFilterCriteria>(x => x.Id == OrderId_Exists
                        || x.CustomerId == CustomerId_Exists
                        || (x.Id == null && x.CustomerId == null && x.OrderStatus == null && x.OrderNumber == null)),
                    It.Is<PageInfo>(p => p.Number == 1 && p.Size == 12)))
                .ReturnsAsync(() => new(ResponseList, ResponseTotal));

            mockRepository
                .Setup(x => x.GetPageByCriteria(
                    It.Is<OrderFilterCriteria>(x => x.Id == OrderId_Exists
                        || x.CustomerId == CustomerId_Exists
                        || (x.Id == null && x.CustomerId == null && x.OrderStatus == null && x.OrderNumber == null)),
                    It.Is<PageInfo>(p => p.Number == 1 && p.Size == 20)))
                .ReturnsAsync(() => new(ResponseList, ResponseTotal));

            mockRepository
                .Setup(x => x.GetPageByCriteria(
                    It.Is<OrderFilterCriteria>(x => x.Id == OrderId_Exists
                        || x.CustomerId == CustomerId_Exists
                        || (x.Id == null && x.CustomerId == null && x.OrderStatus == null && x.OrderNumber == null)),
                    It.Is<PageInfo>(p => p.Number == 2 && p.Size == 3)))
                .ReturnsAsync(() => new([], ResponseTotal));

            return mockRepository;
        }
    }

    [CollectionDefinition("OrderServiceCollection")]
    public class OrderServiceCollection : ICollectionFixture<OrderServiceFixture> { }
}
