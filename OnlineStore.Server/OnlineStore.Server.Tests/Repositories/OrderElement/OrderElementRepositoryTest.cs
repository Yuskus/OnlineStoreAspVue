using OnlineStore.Server.DTO.OrderElements;
using OnlineStore.Server.Repositories.OrderElements;
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

            var request_success = new UpdateOrderElementRequest { ItemPrice = 100, ItemsCount = 1 };

            // Act
            var update_fail_1 = await repository.Update(_fixture.Guid_Unexists, request_success);
            var update_fail_2 = await repository.Update(Guid.Empty, request_success);

            var update_success = await repository.Update(_fixture.OrderElement_ToUpdate, request_success);

            // Assert
            Assert.False(update_fail_1);
            Assert.False(update_fail_2);

            Assert.True(update_success);
        }

        [Fact]
        public async Task Delete()
        {
            // Arrange
            var repository = new OrderElementRepository(_context);

            // Act
            var Delete_Success = await repository.Delete(_fixture.OrderElement_ToDelete);

            var Delete_Fail_1 = await repository.Delete(_fixture.OrderElement_ToDelete);
            var Delete_Fail_2 = await repository.Delete(_fixture.Guid_Unexists);
            var Delete_Fail_3 = await repository.Delete(Guid.Empty);

            // Assert
            Assert.True(Delete_Success);

            Assert.False(Delete_Fail_1);
            Assert.False(Delete_Fail_2);
            Assert.False(Delete_Fail_3);
        }
    }
}
