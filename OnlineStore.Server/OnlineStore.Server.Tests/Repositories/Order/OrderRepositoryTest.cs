using OnlineStore.Server.DTO.Order;
using OnlineStore.Server.Repositories.Order;
using OnlineStore.Server.Tests.Common;

namespace OnlineStore.Server.Tests.Repositories.Order
{
    [Collection("DatabaseCollection")]
    public class OrderRepositoryTest : IClassFixture<OrderDbContextFixture>
    {
        private readonly OrderDbContextFixture _fixture;
        private readonly FakeDbContext _context;
        private readonly OrderGeneratorMock _generatorMock;

        public OrderRepositoryTest(OrderDbContextFixture fixture)
        {
            _fixture = fixture;
            _context = _fixture.Context;
            _generatorMock = new();
        }

        [Fact]
        public async Task GetPageOfOrders()
        {
            // Arrange
            var repository = new OrderRepository(_context, _generatorMock);

            // Act
            var test = await repository.GetAll();

            // Assert
            // there may be range of values because of "create order" test
            Assert.InRange(test.TotalCount, _fixture.OrdersTotalCount - 1, _fixture.OrdersTotalCount + 1);
            Assert.InRange(test.Responses.Count(), _fixture.OrdersTotalCount - 1, _fixture.OrdersTotalCount + 1);
        }

        [Fact]
        public async Task GetPageOfOrdersByCustomerId()
        {
            // Arrange
            var repository = new OrderRepository(_context, _generatorMock);

            // Act
            var unexist = await repository.GetAllByCriteria(new() { CustomerId = _fixture.Guid_Unexists });

            var customerA = await repository.GetAllByCriteria(new() { CustomerId = _fixture.CustomerId_SampleA });
            var customerB = await repository.GetAllByCriteria(new() { CustomerId = _fixture.CustomerId_SampleB });

            // Assert
            Assert.NotNull(unexist);
            Assert.NotNull(customerA);
            Assert.NotNull(customerB);

            Assert.Equal(0, unexist.TotalCount);
            Assert.InRange(customerA.TotalCount, 1, _fixture.OrdersTotalCount);
            Assert.InRange(customerB.TotalCount, 1, _fixture.OrdersTotalCount);

            Assert.Empty(unexist.Responses);
            Assert.InRange(customerA.Responses.Count(), 1, _fixture.OrdersTotalCount);
            Assert.InRange(customerB.Responses.Count(), 1, _fixture.OrdersTotalCount);
        }

        [Fact]
        public async Task GetPageOfOrdersByStatus()
        {
            // Arrange
            var repository = new OrderRepository(_context, _generatorMock);

            // Act
            var unexist = await repository.GetAllByCriteria(new() { OrderStatus = _fixture.Status_Unexists });

            var statusA = await repository.GetAllByCriteria(new() { OrderStatus = _fixture.Status_New });
            var statusB = await repository.GetAllByCriteria(new() { OrderStatus = _fixture.Status_Basket });

            // Assert
            Assert.NotNull(unexist);
            Assert.NotNull(statusA);
            Assert.NotNull(statusB);

            Assert.Equal(0, unexist.TotalCount);
            Assert.InRange(statusA.TotalCount, 1, _fixture.OrdersTotalCount);
            Assert.InRange(statusB.TotalCount, 1, _fixture.OrdersTotalCount);

            Assert.Empty(unexist.Responses);
            Assert.InRange(statusA.Responses.Count(), 1, _fixture.OrdersTotalCount);
            Assert.InRange(statusB.Responses.Count(), 1, _fixture.OrdersTotalCount);
        }

        [Fact]
        public async Task GetOrderByNumber()
        {
            // Arrange
            var repository = new OrderRepository(_context, _generatorMock);

            // Act
            var getByNumber_Fail = await repository.GetOneByCriteria(new() { OrderNumber = _fixture.OrderNumber_Unexists });
            var getByNumber_Success = await repository.GetOneByCriteria(new() { OrderNumber = _fixture.OrderNumber_Exists });

            // Assert
            Assert.Null(getByNumber_Fail);
            Assert.NotNull(getByNumber_Success);
        }

        [Fact]
        public async Task GetBasketOrder()
        {
            // Arrange
            var repository = new OrderRepository(_context, _generatorMock);

            // Act
            var getBasketOrder_Fail = await repository.GetOneByCriteria(new() { CustomerId = _fixture.Guid_Unexists });

            var getBasketOrder_Success1 = await repository.GetOneByCriteria(new() { CustomerId = _fixture.CustomerId_SampleA });
            var getBasketOrder_Success2 = await repository.GetOneByCriteria(new() { CustomerId = _fixture.CustomerId_SampleB });

            // Assert
            Assert.Null(getBasketOrder_Fail);

            Assert.NotNull(getBasketOrder_Success1);
            Assert.NotNull(getBasketOrder_Success2);

            Assert.NotEqual(getBasketOrder_Success1, getBasketOrder_Success2);
        }

        [Fact]
        public async Task CreateOrder()
        {
            // Arrange
            var repository = new OrderRepository(_context, _generatorMock);

            var request = new OrderRequest
            {
                CustomerId = _fixture.CustomerId_SampleA,
                OrderDate = DateOnly.FromDateTime(DateTime.Now).ToString(),
                OrderStatus = "new"
            };

            // Act
            var createNew = await repository.Create(request);

            // Assert
            Assert.NotNull(createNew);
            Assert.NotEqual(createNew, Guid.Empty);
        }

        [Fact]
        public async Task UpdateOrder()
        {
            // Arrange
            var repository = new OrderRepository(_context, _generatorMock);

            var request = new OrderRequest
            {
                CustomerId = _fixture.CustomerId_SampleB,
                OrderDate = DateOnly.FromDateTime(DateTime.Now).ToString(),
                OrderStatus = "new"
            };

            // Act
            var updateOrder_Fail = await repository.Update(_fixture.Guid_Unexists, request);
            var updateOrder_Success = await repository.Update(_fixture.OrderId_ForUpdate, request);

            // Assert
            Assert.False(updateOrder_Fail);
            Assert.True(updateOrder_Success);
        }

        [Fact]
        public async Task DeleteOrder()
        {
            // Arrange
            var repository = new OrderRepository(_context, _generatorMock);

            // Act
            var deleteOrder_Fail = await repository.Delete(_fixture.Guid_Unexists);

            var deleteOrder_Success = await repository.Delete(_fixture.OrderId_ForDelete);
            var deleteOrder_Again = await repository.Delete(_fixture.OrderId_ForDelete);

            // Assert
            Assert.False(deleteOrder_Fail);

            Assert.True(deleteOrder_Success);
            Assert.False(deleteOrder_Again);
        }
    }
}
