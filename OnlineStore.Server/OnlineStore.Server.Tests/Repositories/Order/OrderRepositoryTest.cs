using OnlineStore.Server.DTO.Orders;
using OnlineStore.Server.Repositories.Orders;
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
        public async Task GetAllByCriteria_Success()
        {
            // Arrange
            var repository = new OrderRepository(_context, _generatorMock);

            var request_1 = new OrderFilterCriteria { CustomerId = _fixture.CustomerId_SampleA };
            var request_2 = new OrderFilterCriteria { CustomerId = _fixture.CustomerId_SampleB };
            var request_3 = new OrderFilterCriteria { CustomerId = _fixture.CustomerId_SampleA, OrderStatus = _fixture.Status_New };
            var request_4 = new OrderFilterCriteria { CustomerId = _fixture.CustomerId_SampleA, OrderStatus = _fixture.Status_Basket };
            var request_5 = new OrderFilterCriteria { CustomerId = _fixture.CustomerId_SampleB, OrderStatus = _fixture.Status_New };
            var request_6 = new OrderFilterCriteria { CustomerId = _fixture.CustomerId_SampleB, OrderStatus = _fixture.Status_Basket };

            // Act
            var test_1 = await repository.GetAllByCriteria(request_1);
            var test_2 = await repository.GetAllByCriteria(request_2);
            var test_3 = await repository.GetAllByCriteria(request_3);
            var test_4 = await repository.GetAllByCriteria(request_4);
            var test_5 = await repository.GetAllByCriteria(request_5);
            var test_6 = await repository.GetAllByCriteria(request_6);

            // Assert
            Assert.NotNull(test_1);
            Assert.NotNull(test_2);
            Assert.NotNull(test_3);
            Assert.NotNull(test_4);
            Assert.NotNull(test_5);
            Assert.NotNull(test_6);

            Assert.InRange(test_1.TotalCount, 1, _fixture.OrdersTotalCount);
            Assert.InRange(test_2.TotalCount, 1, _fixture.OrdersTotalCount);
            Assert.InRange(test_3.TotalCount, 1, _fixture.OrdersTotalCount);
            Assert.InRange(test_4.TotalCount, 1, _fixture.OrdersTotalCount);
            Assert.InRange(test_5.TotalCount, 1, _fixture.OrdersTotalCount);
            Assert.InRange(test_6.TotalCount, 1, _fixture.OrdersTotalCount);

            Assert.InRange(test_1.Responses.Count(), 1, _fixture.OrdersTotalCount);
            Assert.InRange(test_2.Responses.Count(), 1, _fixture.OrdersTotalCount);
            Assert.InRange(test_3.Responses.Count(), 1, _fixture.OrdersTotalCount);
            Assert.InRange(test_4.Responses.Count(), 1, _fixture.OrdersTotalCount);
            Assert.InRange(test_5.Responses.Count(), 1, _fixture.OrdersTotalCount);
            Assert.InRange(test_6.Responses.Count(), 1, _fixture.OrdersTotalCount);
        }

        [Fact]
        public async Task GetAllByCriteria_Fail()
        {
            // Arrange
            var repository = new OrderRepository(_context, _generatorMock);

            var request_1 = new OrderFilterCriteria { CustomerId = _fixture.Guid_Unexists };
            var request_2 = new OrderFilterCriteria { OrderStatus = _fixture.Status_Unexists };
            var request_3 = new OrderFilterCriteria { CustomerId = _fixture.CustomerId_SampleA, OrderStatus = _fixture.Status_Unexists };
            var request_4 = new OrderFilterCriteria { CustomerId = _fixture.Guid_Unexists, OrderStatus = _fixture.Status_New };

            // Act
            var test_1 = await repository.GetAllByCriteria(request_1);
            var test_2 = await repository.GetAllByCriteria(request_2);
            var test_3 = await repository.GetAllByCriteria(request_3);
            var test_4 = await repository.GetAllByCriteria(request_4);

            // Assert
            Assert.NotNull(test_1);
            Assert.NotNull(test_2);
            Assert.NotNull(test_3);
            Assert.NotNull(test_4);

            Assert.Equal(0, test_1.TotalCount);
            Assert.Equal(0, test_2.TotalCount);
            Assert.Equal(0, test_3.TotalCount);
            Assert.Equal(0, test_4.TotalCount);

            Assert.Empty(test_1.Responses);
            Assert.Empty(test_2.Responses);
            Assert.Empty(test_3.Responses);
            Assert.Empty(test_4.Responses);
        }

        [Fact]
        public async Task GetOneByCriteria()
        {
            // Arrange
            var repository = new OrderRepository(_context, _generatorMock);

            var request_fail = new OrderFilterCriteria { OrderNumber = _fixture.OrderNumber_Unexists };
            var request_success = new OrderFilterCriteria { OrderNumber = _fixture.OrderNumber_Exists };

            // Act
            var getByCriteria_Fail = await repository.GetOneByCriteria(request_fail);
            var getByCriteria_Success = await repository.GetOneByCriteria(request_success);

            // Assert
            Assert.Null(getByCriteria_Fail);
            Assert.NotNull(getByCriteria_Success);
        }

        [Fact]
        public async Task GetBasketOrder()
        {
            // Arrange
            var repository = new OrderRepository(_context, _generatorMock);

            var request_fail = new OrderFilterCriteria { CustomerId = _fixture.Guid_Unexists, OrderStatus = _fixture.Status_Basket };
            var request_success_1 = new OrderFilterCriteria { CustomerId = _fixture.CustomerId_SampleA, OrderStatus = _fixture.Status_Basket };
            var request_success_2 = new OrderFilterCriteria { CustomerId = _fixture.CustomerId_SampleB, OrderStatus = _fixture.Status_Basket };

            // Act
            var getBasketOrder_Fail = await repository.GetOneByCriteria(request_fail);

            var getBasketOrder_Success1 = await repository.GetOneByCriteria(request_success_1);
            var getBasketOrder_Success2 = await repository.GetOneByCriteria(request_success_2);

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

            var request_fail = new OrderRequest
            {
                CustomerId = _fixture.Guid_Unexists,
                OrderDate = DateOnly.FromDateTime(DateTime.Now).ToString(),
                OrderStatus = "new"
            };

            var request_success = new OrderRequest
            {
                CustomerId = _fixture.CustomerId_SampleA,
                OrderDate = DateOnly.FromDateTime(DateTime.Now).ToString(),
                OrderStatus = "new"
            };

            // Act
            var createNew_fail = await repository.Create(request_fail);
            var createNew_success = await repository.Create(request_success);

            // Assert
            Assert.Null(createNew_fail);

            Assert.NotNull(createNew_success);
            Assert.NotEqual(createNew_success, Guid.Empty);
        }

        [Fact]
        public async Task UpdateOrder()
        {
            // Arrange
            var repository = new OrderRepository(_context, _generatorMock);

            var request_fail = new OrderRequest
            {
                CustomerId = _fixture.Guid_Unexists,
                OrderDate = DateOnly.FromDateTime(DateTime.Now).ToString(),
                OrderStatus = "new"
            };

            var request_success = new OrderRequest
            {
                CustomerId = _fixture.CustomerId_SampleB,
                OrderDate = DateOnly.FromDateTime(DateTime.Now).ToString(),
                OrderStatus = "new"
            };

            // Act
            var updateOrder_Fail_1 = await repository.Update(_fixture.Guid_Unexists, request_fail);
            var updateOrder_Fail_2 = await repository.Update(_fixture.OrderId_ForUpdate, request_fail);
            var updateOrder_Fail_3 = await repository.Update(_fixture.Guid_Unexists, request_success);

            var updateOrder_Success = await repository.Update(_fixture.OrderId_ForUpdate, request_success);

            // Assert
            Assert.False(updateOrder_Fail_1);
            Assert.False(updateOrder_Fail_2);
            Assert.False(updateOrder_Fail_3);

            Assert.True(updateOrder_Success);
        }

        [Fact]
        public async Task DeleteOrder()
        {
            // Arrange
            var repository = new OrderRepository(_context, _generatorMock);

            // Act
            var deleteOrder_Fail_1 = await repository.Delete(_fixture.Guid_Unexists);
            var deleteOrder_Success = await repository.Delete(_fixture.OrderId_ForDelete);
            var deleteOrder_Fail_2 = await repository.Delete(_fixture.OrderId_ForDelete);

            // Assert
            Assert.False(deleteOrder_Fail_1);
            Assert.False(deleteOrder_Fail_2);

            Assert.True(deleteOrder_Success);
        }
    }
}
