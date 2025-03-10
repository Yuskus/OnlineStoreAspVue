using OnlineStore.Server.DTO.OrderElement;
using OnlineStore.Server.Repositories.OrderElement;
using OnlineStore.Server.Tests.Common;

namespace OnlineStore.Server.Tests.Repositories.OrderElement
{
    [Collection("DatabaseCollection")]
    public class OrderElementRepositoryTest : IClassFixture<OrderElementDbContextFixture>
    {
        private readonly FakeDbContext _context;
        private readonly OrderElementDbContextFixture _fixture;

        public OrderElementRepositoryTest(OrderElementDbContextFixture fixture)
        {
            _fixture = fixture;
            _context = _fixture.Context;
        }

        [Fact]
        public async Task GetAllByOrderId()
        {
            // Arrange
            var repository = new OrderElementRepository(_context);

            // Act
            var GetAllByOrderId_Fail = await repository.GetAllByOrderId(_fixture.Guid_Unexists);
            var GetAllByOrderId_Success = await repository.GetAllByOrderId(_fixture.OrderId_SampleA);

            // Assert
            Assert.Empty(GetAllByOrderId_Fail);
            Assert.NotEmpty(GetAllByOrderId_Success);
        }

        [Fact]
        public async Task Create()
        {
            // Arrange
            var repository = new OrderElementRepository(_context);

            var request_Fail_1 = new OrderElementRequest
            {
                OrderId = _fixture.OrderId_SampleB,
                ItemId = _fixture.Guid_Unexists,
                ItemPrice = 100,
                ItemsCount = 1
            };

            var request_Fail_2 = new OrderElementRequest
            {
                OrderId = _fixture.Guid_Unexists,
                ItemId = _fixture.ItemId_SampleA,
                ItemPrice = 100,
                ItemsCount = 1
            };

            var request_Fail_3 = new OrderElementRequest
            {
                OrderId = _fixture.Guid_Unexists,
                ItemId = _fixture.Guid_Unexists,
                ItemPrice = 100,
                ItemsCount = 1
            };

            var request_Success = new OrderElementRequest
            {
                OrderId = _fixture.OrderId_SampleB,
                ItemId = _fixture.ItemId_SampleA,
                ItemPrice = 100,
                ItemsCount = 1
            };

            // Act
            var Create_Fail_1 = await repository.Create(request_Fail_1);
            var Create_Fail_2 = await repository.Create(request_Fail_2);
            var Create_Fail_3 = await repository.Create(request_Fail_3);

            var Create_Success = await repository.Create(request_Success);

            // Assert
            Assert.Null(Create_Fail_1);
            Assert.Null(Create_Fail_2);
            Assert.Null(Create_Fail_3);

            Assert.NotNull(Create_Success);
            Assert.NotEqual(Create_Success, Guid.Empty);
        }

        [Fact]
        public async Task Update()
        {
            // Arrange
            var repository = new OrderElementRepository(_context);

            var request_Fail_1 = new OrderElementRequest
            {
                OrderId = _fixture.OrderId_SampleB,
                ItemId = _fixture.Guid_Unexists,
                ItemPrice = 100,
                ItemsCount = 1
            };

            var request_Fail_2 = new OrderElementRequest
            {
                OrderId = _fixture.Guid_Unexists,
                ItemId = _fixture.ItemId_SampleA,
                ItemPrice = 100,
                ItemsCount = 1
            };

            var request_Fail_3 = new OrderElementRequest
            {
                OrderId = _fixture.Guid_Unexists,
                ItemId = _fixture.Guid_Unexists,
                ItemPrice = 100,
                ItemsCount = 1
            };

            var request_Success = new OrderElementRequest
            {
                OrderId = _fixture.OrderId_SampleA,
                ItemId = _fixture.ItemId_SampleA,
                ItemPrice = 100,
                ItemsCount = 1
            };

            // Act
            var Update_Fail_1 = await repository.Update(_fixture.Guid_Unexists, request_Fail_1);
            var Update_Fail_2 = await repository.Update(_fixture.OrderElement_ToUpdate, request_Fail_1);

            var Update_Fail_3 = await repository.Update(_fixture.Guid_Unexists, request_Fail_2);
            var Update_Fail_4 = await repository.Update(_fixture.OrderElement_ToUpdate, request_Fail_2);

            var Update_Fail_5 = await repository.Update(_fixture.Guid_Unexists, request_Fail_3);
            var Update_Fail_6 = await repository.Update(_fixture.OrderElement_ToUpdate, request_Fail_3);

            var Update_Success = await repository.Update(_fixture.OrderElement_ToUpdate, request_Success);

            // Assert
            Assert.False(Update_Fail_1);
            Assert.False(Update_Fail_2);
            Assert.False(Update_Fail_3);
            Assert.False(Update_Fail_4);
            Assert.False(Update_Fail_5);
            Assert.False(Update_Fail_6);

            Assert.True(Update_Success);
        }

        [Fact]
        public async Task Delete()
        {
            // Arrange
            var repository = new OrderElementRepository(_context);

            // Act
            var Delete_Fail_1 = await repository.Delete(_fixture.Guid_Unexists);
            var Delete_Success = await repository.Delete(_fixture.OrderElement_ToDelete);
            var Delete_Fail_2 = await repository.Delete(_fixture.OrderElement_ToDelete);

            // Assert
            Assert.False(Delete_Fail_1);
            Assert.False(Delete_Fail_2);

            Assert.True(Delete_Success);
        }
    }
}
