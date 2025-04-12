using Moq;
using OnlineStore.Server.DTO.OrderElement;
using OnlineStore.Server.Repositories.OrderElement;
using OnlineStore.Server.Services.OrderElement;

namespace OnlineStore.Server.Tests.Services.OrderElement
{
    [Collection("OrderElementServiceCollection")]
    public class OrderElementServiceTest : IClassFixture<OrderElementServiceFixture>
    {
        /*Task<bool> Update(Guid id, OrderElementRequest orderElement);
        Task<bool> Delete(Guid id);
        Task<IEnumerable<OrderElementResponse>> GetAllByOrderId(Guid id);*/

        private readonly OrderElementServiceFixture _fixture;
        private readonly Mock<IOrderElementRepository> _mockRepository;

        public OrderElementServiceTest(OrderElementServiceFixture fixture)
        {
            _fixture = fixture;
            _mockRepository = _fixture.CreateMockRepository();
        }

        [Fact]
        public async Task Create_Success()
        {
            //Arrange
            var service = new OrderElementService(_mockRepository.Object);

            var request_success_1 = new OrderElementRequest //yes, yes, yes, yes
            {
                OrderId = _fixture.OrderId_Exists,
                ItemId = _fixture.ItemId_Exists,
                ItemPrice = _fixture.ItemPrice_Exists[0],
                ItemsCount = _fixture.ItemsCount_Exists[0]
            };

            var request_success_2 = new OrderElementRequest //yes, yes, yes, yes
            {
                OrderId = _fixture.OrderId_Exists,
                ItemId = _fixture.ItemId_Exists,
                ItemPrice = _fixture.ItemPrice_Exists[0],
                ItemsCount = _fixture.ItemsCount_Exists[1]
            };
            
            //Act
            var create_success_1 = await service.Create(request_success_1);
            var create_success_2 = await service.Create(request_success_2);

            //Assert
            Assert.NotNull(create_success_1);
            Assert.NotNull(create_success_2);

            Assert.NotEqual(Guid.Empty, create_success_1);
            Assert.NotEqual(Guid.Empty, create_success_2);

            Assert.Equal(create_success_1, create_success_2);
        }

        [Fact]
        public async Task Create_Fail()
        {
            //Arrange
            var service = new OrderElementService(_mockRepository.Object);

            var request_fail_1 = new OrderElementRequest // no, yes, yes, yes
            {
                OrderId = _fixture.Guid_Unexists,
                ItemId = _fixture.ItemId_Exists,
                ItemPrice = _fixture.ItemPrice_Exists[0],
                ItemsCount = _fixture.ItemsCount_Exists[1]
            };

            var request_fail_2 = new OrderElementRequest // yes, no, yes, yes
            {
                OrderId = _fixture.OrderId_Exists,
                ItemId = _fixture.Guid_Unexists,
                ItemPrice = _fixture.ItemPrice_Exists[1],
                ItemsCount = _fixture.ItemsCount_Exists[0]
            };

            var request_fail_3 = new OrderElementRequest // no, no, yes, yes
            {
                OrderId = _fixture.Guid_Unexists,
                ItemId = _fixture.Guid_Unexists,
                ItemPrice = _fixture.ItemPrice_Exists[0],
                ItemsCount = _fixture.ItemsCount_Exists[0]
            };

            var request_fail_4 = new OrderElementRequest // yes, yes, no, yes
            {
                OrderId = _fixture.OrderId_Exists,
                ItemId = _fixture.ItemId_Exists,
                ItemPrice = _fixture.ItemPrice_Unexists[0],
                ItemsCount = _fixture.ItemsCount_Exists[0]
            };

            var request_fail_5 = new OrderElementRequest // yes, yes, no, yes
            {
                OrderId = _fixture.OrderId_Exists,
                ItemId = _fixture.ItemId_Exists,
                ItemPrice = _fixture.ItemPrice_Unexists[1],
                ItemsCount = _fixture.ItemsCount_Exists[1]
            };

            var request_fail_6 = new OrderElementRequest // yes, yes, yes, no
            {
                OrderId = _fixture.OrderId_Exists,
                ItemId = _fixture.ItemId_Exists,
                ItemPrice = _fixture.ItemPrice_Exists[0],
                ItemsCount = _fixture.ItemsCount_Unexists[0]
            };

            var request_fail_7 = new OrderElementRequest // yes, yes, yes, no
            {
                OrderId = _fixture.OrderId_Exists,
                ItemId = _fixture.ItemId_Exists,
                ItemPrice = _fixture.ItemPrice_Exists[1],
                ItemsCount = _fixture.ItemsCount_Unexists[1]
            };

            //Act
            var create_fail_1 = await service.Create(request_fail_1);
            var create_fail_2 = await service.Create(request_fail_2);
            var create_fail_3 = await service.Create(request_fail_3);
            var create_fail_4 = await service.Create(request_fail_4);
            var create_fail_5 = await service.Create(request_fail_5);
            var create_fail_6 = await service.Create(request_fail_6);
            var create_fail_7 = await service.Create(request_fail_7);

            //Assert
            Assert.Null(create_fail_1);
            Assert.Null(create_fail_2);
            Assert.Null(create_fail_3);
            Assert.Null(create_fail_4);
            Assert.Null(create_fail_5);
            Assert.Null(create_fail_6);
            Assert.Null(create_fail_7);
        }
    }
}