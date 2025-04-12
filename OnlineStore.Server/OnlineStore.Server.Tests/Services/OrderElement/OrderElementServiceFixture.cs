using Moq;
using OnlineStore.Server.DTO.OrderElement;
using OnlineStore.Server.Repositories.OrderElement;

namespace OnlineStore.Server.Tests.Services.OrderElement
{
    public class OrderElementServiceFixture
    {
        public Guid Guid_Unexists { get; private set; } = Guid.NewGuid();
        public Guid OrderId_Exists { get; private set; } = Guid.NewGuid();
        public Guid ItemId_Exists { get; private set; } = Guid.NewGuid();
        public Guid OrderElementId_Exists { get; private set; } = Guid.NewGuid();
        public int[] ItemsCount_Exists { get; private set; } = [1, 100];
        public double[] ItemPrice_Exists { get; private set; } = [1, 9999.9999];
        public int[] ItemsCount_Unexists { get; private set; } = [-1, 0];
        public double[] ItemPrice_Unexists { get; private set; } = [-1.2, 0];

        public Mock<IOrderElementRepository> CreateMockRepository()
        {
            var mockRepository = new Mock<IOrderElementRepository>();

            // create

            mockRepository.Setup(x => x.Create(It.Is<OrderElementRequest>(x => x.ItemId == ItemId_Exists && x.OrderId == OrderId_Exists)))
                          .ReturnsAsync(OrderElementId_Exists);

            mockRepository.Setup(x => x.Create(It.Is<OrderElementRequest>(x => x.ItemId == Guid_Unexists || x.OrderId == Guid_Unexists)))
                          .ReturnsAsync(() => null);

            /*//update

            mockRepository.Setup(x => x.Update(CustomerId_Exists, It.IsAny<OrderElementRequest>())).ReturnsAsync(true);

            mockRepository.Setup(x => x.Update(Guid_Unexists, It.IsAny<OrderElementRequest>())).ReturnsAsync(false);

            mockRepository.Setup(x => x.Update(OrderId_Exists, It.IsAny<OrderElementRequest>())).ReturnsAsync(true);*/

            return mockRepository;
        }
    }

    [CollectionDefinition("OrderElementServiceCollection")]
    public class OrderElementServiceCollection : ICollectionFixture<OrderElementServiceFixture> { }
}
