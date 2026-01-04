using Moq;
using OnlineStore.Server.DTO.OrderElements;
using OnlineStore.Server.Repositories.OrderElements;

namespace OnlineStore.Server.Tests.Services.OrderElement
{
    public class OrderElementServiceFixture
    {
        public Guid Guid_Unexists { get; private set; } = Guid.NewGuid();
        public Guid OrderId_Exists { get; private set; } = Guid.NewGuid();
        public Guid ItemId_Exists { get; private set; } = Guid.NewGuid();
        public Guid[] OrderElementId_Exists { get; private set; } = [Guid.NewGuid(), Guid.NewGuid()];
        public int[] ItemsCount_Exists { get; private set; } = [1, 100];
        public double[] ItemPrice_Exists { get; private set; } = [1, 9999.9999];
        public int[] ItemsCount_Unexists { get; private set; } = [-1, 0];
        public double[] ItemPrice_Unexists { get; private set; } = [-1.2, 0];
        public List<OrderElementResponse> OrderElements_ForGetAllByOrderId { get; private set; } = [
            new()
            {
                Id = Guid.NewGuid(),
                OrderId = Guid.NewGuid(),
                ItemId = Guid.NewGuid(),
                ItemPrice = 100,
                ItemsCount = 1
            },
            new()
            {
                Id = Guid.NewGuid(),
                OrderId = Guid.NewGuid(),
                ItemId = Guid.NewGuid(),
                ItemPrice = 200,
                ItemsCount = 2
            },
            new()
            {
                Id = Guid.NewGuid(),
                OrderId = Guid.NewGuid(),
                ItemId = Guid.NewGuid(),
                ItemPrice = 300,
                ItemsCount = 3
            },
            new()
            {
                Id = Guid.NewGuid(),
                OrderId = Guid.NewGuid(),
                ItemId = Guid.NewGuid(),
                ItemPrice = 400,
                ItemsCount = 2
            }
        ];

        public Mock<IOrderElementRepository> CreateMockRepository()
        {
            var mockRepository = new Mock<IOrderElementRepository>();

            // create

            mockRepository.Setup(x => x.Create(It.Is<OrderElementRequest>(x => x.ItemId == ItemId_Exists && x.OrderId == OrderId_Exists)))
                          .ReturnsAsync(OrderElementId_Exists[0]);

            mockRepository.Setup(x => x.Create(It.Is<OrderElementRequest>(x => x.ItemId == Guid_Unexists || x.OrderId == Guid_Unexists)))
                          .ReturnsAsync(() => null);

            //update

            mockRepository.Setup(x => x.Update(OrderElementId_Exists[0], It.IsAny<UpdateOrderElementRequest>())).ReturnsAsync(true);

            mockRepository.Setup(x => x.Update(OrderElementId_Exists[1], It.IsAny<UpdateOrderElementRequest>())).ReturnsAsync(true);

            mockRepository.Setup(x => x.Update(Guid_Unexists, It.IsAny<UpdateOrderElementRequest>())).ReturnsAsync(false);

            //delete

            mockRepository.SetupSequence(x => x.Delete(OrderElementId_Exists[0])).ReturnsAsync(true).ReturnsAsync(false);

            mockRepository.SetupSequence(x => x.Delete(OrderElementId_Exists[1])).ReturnsAsync(true).ReturnsAsync(false);

            mockRepository.Setup(x => x.Delete(Guid_Unexists)).ReturnsAsync(false);

            // get

            mockRepository.Setup(x => x.GetAllByOrderId(OrderId_Exists)).ReturnsAsync(() => OrderElements_ForGetAllByOrderId);

            mockRepository.Setup(x => x.GetAllByOrderId(Guid_Unexists)).ReturnsAsync([]);

            return mockRepository;
        }
    }

    [CollectionDefinition("OrderElementServiceCollection")]
    public class OrderElementServiceCollection : ICollectionFixture<OrderElementServiceFixture>
    { }
}