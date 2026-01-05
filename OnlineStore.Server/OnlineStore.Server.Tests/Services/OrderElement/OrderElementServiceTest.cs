using Moq;
using OnlineStore.Server.DTO.OrderElements;
using OnlineStore.Server.Repositories.OrderElements;
using OnlineStore.Server.Services.OrderElements;

namespace OnlineStore.Server.Tests.Services.OrderElement
{
    [Collection("OrderElementServiceCollection")]
    public class OrderElementServiceTest : IClassFixture<OrderElementServiceFixture>
    {
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
                ItemPrice = _fixture.ItemPrice_Exists[1],
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

        [Fact]
        public async Task Update_Success()
        {
            //Arrange
            var service = new OrderElementService(_mockRepository.Object);

            var request_success_1 = new UpdateOrderElementRequest //yy
            {
                ItemPrice = _fixture.ItemPrice_Exists[0],
                ItemsCount = _fixture.ItemsCount_Exists[0]
            };

            var request_success_2 = new UpdateOrderElementRequest //yy
            {
                ItemPrice = _fixture.ItemPrice_Exists[1],
                ItemsCount = _fixture.ItemsCount_Exists[1]
            };

            //Act
            var update_success_1 = await service.Update(_fixture.OrderElementId_Exists[0], request_success_1);
            var update_success_2 = await service.Update(_fixture.OrderElementId_Exists[1], request_success_2);
            var update_success_3 = await service.Update(_fixture.OrderElementId_Exists[0], request_success_2);
            var update_success_4 = await service.Update(_fixture.OrderElementId_Exists[1], request_success_1);

            //Assert
            Assert.True(update_success_1);
            Assert.True(update_success_2);
            Assert.True(update_success_3);
            Assert.True(update_success_4);
        }

        [Fact]
        public async Task Update_Fail()
        {
            //Arrange
            var service = new OrderElementService(_mockRepository.Object);

            var request_fake_success_1 = new UpdateOrderElementRequest //yy
            {
                ItemPrice = _fixture.ItemPrice_Exists[0],
                ItemsCount = _fixture.ItemsCount_Exists[0]
            };

            var request_fake_success_2 = new UpdateOrderElementRequest //yy
            {
                ItemPrice = _fixture.ItemPrice_Exists[1],
                ItemsCount = _fixture.ItemsCount_Exists[1]
            };

            var request_fail_1 = new UpdateOrderElementRequest //ny
            {
                ItemPrice = _fixture.ItemPrice_Unexists[0],
                ItemsCount = _fixture.ItemsCount_Exists[0]
            };

            var request_fail_2 = new UpdateOrderElementRequest //yn
            {
                ItemPrice = _fixture.ItemPrice_Exists[0],
                ItemsCount = _fixture.ItemsCount_Unexists[0]
            };

            var request_fail_3 = new UpdateOrderElementRequest //ny
            {
                ItemPrice = _fixture.ItemPrice_Unexists[1],
                ItemsCount = _fixture.ItemsCount_Exists[1]
            };

            var request_fail_4 = new UpdateOrderElementRequest //yn
            {
                ItemPrice = _fixture.ItemPrice_Exists[1],
                ItemsCount = _fixture.ItemsCount_Unexists[1]
            };

            //Act
            var update_fail_1 = await service.Update(_fixture.Guid_Unexists, request_fake_success_1); //n
            var update_fail_2 = await service.Update(_fixture.Guid_Unexists, request_fake_success_2); //n

            var update_fail_3 = await service.Update(_fixture.OrderElementId_Exists[0], request_fail_1); //y
            var update_fail_4 = await service.Update(_fixture.OrderElementId_Exists[1], request_fail_2); //y
            var update_fail_5 = await service.Update(_fixture.OrderElementId_Exists[0], request_fail_3); //y
            var update_fail_6 = await service.Update(_fixture.OrderElementId_Exists[1], request_fail_4); //y

            //Assert
            Assert.False(update_fail_1);
            Assert.False(update_fail_2);

            Assert.False(update_fail_3);
            Assert.False(update_fail_4);
            Assert.False(update_fail_5);
            Assert.False(update_fail_6);
        }

        [Fact]
        public async Task Delete()
        {
            //Arrange
            var service = new OrderElementService(_mockRepository.Object);

            //Act
            var delete_success_1 = await service.Delete(_fixture.OrderElementId_Exists[0]);
            var delete_success_2 = await service.Delete(_fixture.OrderElementId_Exists[1]);

            var delete_fail_1 = await service.Delete(_fixture.OrderElementId_Exists[0]);
            var delete_fail_2 = await service.Delete(_fixture.OrderElementId_Exists[1]);
            var delete_fail_3 = await service.Delete(_fixture.Guid_Unexists);
            var delete_fail_4 = await service.Delete(Guid.Empty);

            //Assert
            _mockRepository.Verify(x => x.Delete(_fixture.OrderElementId_Exists[0]), Times.Exactly(2));
            _mockRepository.Verify(x => x.Delete(_fixture.OrderElementId_Exists[1]), Times.Exactly(2));

            Assert.True(delete_success_1);
            Assert.True(delete_success_2);

            Assert.False(delete_fail_1);
            Assert.False(delete_fail_2);
            Assert.False(delete_fail_3);
            Assert.False(delete_fail_4);
        }

        [Fact]
        public async Task GetAllByOrderId()
        {
            //Arrange
            var service = new OrderElementService(_mockRepository.Object);

            var getall_success = await service.GetAllByOrderId(_fixture.OrderId_Exists);

            var getall_fail_1 = await service.GetAllByOrderId(_fixture.Guid_Unexists);
            var getall_fail_2 = await service.GetAllByOrderId(Guid.Empty);

            Assert.NotNull(getall_success);

            Assert.NotNull(getall_fail_1);
            Assert.NotNull(getall_fail_2);

            Assert.Equal(_fixture.OrderElements_ForGetAllByOrderId.Count(), getall_success.Count());

            Assert.Empty(getall_fail_1);
            Assert.Empty(getall_fail_2);
        }
    }
}