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
            var test1 = await repository.GetPageOfOrders(1, 5);
            var test2 = await repository.GetPageOfOrders(2, 5);
            var test3 = await repository.GetPageOfOrders(3, 3);
            var test4 = await repository.GetPageOfOrders(1, 20);
            var test5 = await repository.GetPageOfOrders(5, 1);

            // Assert
            // there may be range of values because of "create order" test
            Assert.InRange(test1.TotalCount, _fixture.OrdersTotalCount - 1, _fixture.OrdersTotalCount + 1);
            Assert.InRange(test2.TotalCount, _fixture.OrdersTotalCount - 1, _fixture.OrdersTotalCount + 1);
            Assert.InRange(test3.TotalCount, _fixture.OrdersTotalCount - 1, _fixture.OrdersTotalCount + 1);
            Assert.InRange(test4.TotalCount, _fixture.OrdersTotalCount - 1, _fixture.OrdersTotalCount + 1);
            Assert.InRange(test5.TotalCount, _fixture.OrdersTotalCount - 1, _fixture.OrdersTotalCount + 1);

            Assert.Equal(5, test1.Responses.Count());
            Assert.Equal(5, test2.Responses.Count());
            Assert.Equal(3, test3.Responses.Count());
            Assert.Equal(20, test4.Responses.Count());
            Assert.Single(test5.Responses);
        }

        [Fact]
        public async Task GetPageOfOrdersByCustomerId()
        {
            // Arrange
            var repository = new OrderRepository(_context, _generatorMock);

            // Act
            var unexist = await repository.GetPageOfOrdersByCustomerId(_fixture.Guid_Unexists, 1, 5);

            var customerA = await repository.GetPageOfOrdersByCustomerId(_fixture.CustomerId_SampleA, 1, 5);
            var customerB = await repository.GetPageOfOrdersByCustomerId(_fixture.CustomerId_SampleB, 1, 7);

            // Assert
            Assert.NotNull(unexist);
            Assert.NotNull(customerA);
            Assert.NotNull(customerB);

            Assert.Equal(0, unexist.TotalCount);
            Assert.InRange(customerA.TotalCount, 1, _fixture.OrdersTotalCount);
            Assert.InRange(customerB.TotalCount, 1, _fixture.OrdersTotalCount);

            Assert.Empty(unexist.Responses);
            Assert.InRange(customerA.Responses.Count(), 1, 5);
            Assert.InRange(customerB.Responses.Count(), 1, 7);
        }

        [Fact]
        public async Task GetPageOfOrdersByStatus()
        {
            // Arrange
            var repository = new OrderRepository(_context, _generatorMock);

            // Act
            var unexist = await repository.GetPageOfOrdersByStatus(_fixture.Status_Unexists, 1, 5);

            var statusA = await repository.GetPageOfOrdersByStatus(_fixture.Status_New, 1, 5);
            var statusB = await repository.GetPageOfOrdersByStatus(_fixture.Status_Basket, 1, 7);

            // Assert
            Assert.NotNull(unexist);
            Assert.NotNull(statusA);
            Assert.NotNull(statusB);

            Assert.Equal(0, unexist.TotalCount);
            Assert.InRange(statusA.TotalCount, 1, _fixture.OrdersTotalCount);
            Assert.InRange(statusB.TotalCount, 1, _fixture.OrdersTotalCount);

            Assert.Empty(unexist.Responses);
            Assert.InRange(statusA.Responses.Count(), 1, 5);
            Assert.InRange(statusB.Responses.Count(), 1, 7);
        }

        [Fact]
        public async Task GetOrderByNumber()
        {
            // Arrange
            var repository = new OrderRepository(_context, _generatorMock);

            // Act
            var getByNumber_Fail = await repository.GetOrderByNumber(_fixture.OrderNumber_Unexists);
            var getByNumber_Success = await repository.GetOrderByNumber(_fixture.OrderNumber_Exists);

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
            var getBasketOrder_Fail = await repository.GetBasketOrder(_fixture.Guid_Unexists);

            var getBasketOrder_Success1 = await repository.GetBasketOrder(_fixture.CustomerId_SampleA);
            var getBasketOrder_Success2 = await repository.GetBasketOrder(_fixture.CustomerId_SampleB);

            // Assert
            Assert.Null(getBasketOrder_Fail);

            Assert.NotNull(getBasketOrder_Success1);
            Assert.NotNull(getBasketOrder_Success2);

            Assert.NotEqual(getBasketOrder_Success1, getBasketOrder_Success2);
        }

        [Fact]
        public async Task PlaceAnOrder()
        {
            // Arrange
            var repository = new OrderRepository(_context, _generatorMock);

            // Act
            var placeAnOrder_Fail = await repository.PlaceAnOrder(_fixture.OrderId_StatusNew);
            var placeAnOrder_Success = await repository.PlaceAnOrder(_fixture.OrderId_StatusBasket);

            // Assert
            Assert.False(placeAnOrder_Fail);
            Assert.True(placeAnOrder_Success);
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
            var createNew = await repository.CreateOrder(request);

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
            var updateOrder_Fail = await repository.UpdateOrder(_fixture.Guid_Unexists, request);
            var updateOrder_Success = await repository.UpdateOrder(_fixture.OrderId_ForUpdate, request);

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
            var deleteOrder_Fail = await repository.DeleteOrder(_fixture.Guid_Unexists);

            var deleteOrder_Success = await repository.DeleteOrder(_fixture.OrderId_ForDelete);
            var deleteOrder_Again = await repository.DeleteOrder(_fixture.OrderId_ForDelete);

            // Assert
            Assert.False(deleteOrder_Fail);

            Assert.True(deleteOrder_Success);
            Assert.False(deleteOrder_Again);
        }
    }
}
