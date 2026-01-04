using Moq;
using OnlineStore.Server.Constants.Orders;
using OnlineStore.Server.Database.Entities;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Orders;
using OnlineStore.Server.Repositories.Orders;
using OnlineStore.Server.Services.Orders;

namespace OnlineStore.Server.Tests.Services.Order
{
    [Collection("OrderServiceCollection")]
    public class OrderServiceTest : IClassFixture<OrderServiceFixture>
    {
        private readonly OrderServiceFixture _fixture;
        private readonly Mock<IOrderRepository> _mockRepository;

        public OrderServiceTest(OrderServiceFixture fixture)
        {
            _fixture = fixture;
            _mockRepository = _fixture.CreateMockRepository();
        }

        [Fact]
        public async Task Create_Success()
        {
            //Arrange
            var service = new OrderService(_mockRepository.Object);

            var request_success_1 = new OrderRequest { CustomerId = _fixture.CustomerId_Exists, OrderDate = _fixture.Today.AddDays(10).ToString() }; //yes, yes, (yes, yes)
            var request_success_2 = new OrderRequest { CustomerId = _fixture.CustomerId_Exists, OrderDate = _fixture.Today.AddDays(-10).ToString() }; //yes, yes, (yes, yes)
            var request_success_3 = new OrderRequest { CustomerId = _fixture.CustomerId_Exists, OrderDate = _fixture.Today.ToString(), ShipmentDate = _fixture.Today.AddDays(10).ToString(), OrderStatus = OrderStatuses.New }; //yes, yes, yes, yes
            
            //Act
            var create_success_1 = await service.Create(request_success_1);
            var create_success_2 = await service.Create(request_success_2);
            var create_success_3 = await service.Create(request_success_3);

            //Assert
            Assert.NotNull(create_success_1);
            Assert.NotNull(create_success_2);
            Assert.NotNull(create_success_3);

            Assert.NotEqual(Guid.Empty, create_success_1);
            Assert.NotEqual(Guid.Empty, create_success_2);
            Assert.NotEqual(Guid.Empty, create_success_3);

            Assert.Equal(create_success_1, create_success_2);
            Assert.Equal(create_success_2, create_success_3);
        }

        [Fact]
        public async Task Create_Fail()
        {
            //Arrange
            var service = new OrderService(_mockRepository.Object);

            var request_fail_1 = new OrderRequest { CustomerId = _fixture.CustomerId_Exists, OrderDate = "99-99-9999" }; //yes, no, (yes, yes)
            var request_fail_2 = new OrderRequest { CustomerId = _fixture.CustomerId_Exists, OrderDate = _fixture.Today.ToString(), ShipmentDate = _fixture.Today.AddDays(-10).ToString() }; //yes, yes, no, (yes)
            var request_fail_3 = new OrderRequest { CustomerId = _fixture.CustomerId_Exists, OrderDate = _fixture.Today.AddDays(-10).ToString(), ShipmentDate = _fixture.Today.ToString(), OrderStatus = "unexists" }; //yes, yes, yes, no
            var request_fail_4 = new OrderRequest { CustomerId = _fixture.Guid_Unexists, OrderDate = _fixture.Today.ToString(), ShipmentDate = _fixture.Today.AddDays(-10).ToString(), OrderStatus = OrderStatuses.New }; //no, yes, yes, yes

            //Act
            var create_fail_1 = await service.Create(request_fail_1);
            var create_fail_2 = await service.Create(request_fail_2);
            var create_fail_3 = await service.Create(request_fail_3);
            var create_fail_4 = await service.Create(request_fail_4);

            //Assert
            Assert.Null(create_fail_1);
            Assert.Null(create_fail_2);
            Assert.Null(create_fail_3);
            Assert.Null(create_fail_4);
        }

        [Fact]
        public async Task Update_Success()
        {
            //Arrange
            var service = new OrderService(_mockRepository.Object);

            var request_success_1 = new OrderRequest { CustomerId = _fixture.CustomerId_Exists, OrderDate = _fixture.Today.AddDays(10).ToString() }; //yes, yes, (yes, yes)
            var request_success_2 = new OrderRequest { CustomerId = _fixture.CustomerId_Exists, OrderDate = _fixture.Today.AddDays(-10).ToString() }; //yes, yes, (yes, yes)
            var request_success_3 = new OrderRequest { CustomerId = _fixture.CustomerId_Exists, OrderDate = _fixture.Today.ToString(), ShipmentDate = _fixture.Today.AddDays(10).ToString(), OrderStatus = OrderStatuses.New }; //yes, yes, yes, yes

            //Act
            var update_success_1 = await service.Update(_fixture.OrderId_Exists, request_success_1);
            var update_success_2 = await service.Update(_fixture.OrderId_Exists, request_success_2);
            var update_success_3 = await service.Update(_fixture.OrderId_Exists, request_success_3);

            //Assert
            Assert.True(update_success_1);
            Assert.True(update_success_2);
            Assert.True(update_success_3);
        }

        [Fact]
        public async Task Update_Fail()
        {
            //Arrange
            var service = new OrderService(_mockRepository.Object);

            var request_fake_success_1 = new OrderRequest { CustomerId = _fixture.CustomerId_Exists, OrderDate = _fixture.Today.AddDays(10).ToString() }; //yes, yes, (yes, yes)
            var request_fake_success_2 = new OrderRequest { CustomerId = _fixture.CustomerId_Exists, OrderDate = _fixture.Today.AddDays(-10).ToString() }; //yes, yes, (yes, yes)
            var request_fake_success_3 = new OrderRequest { CustomerId = _fixture.CustomerId_Exists, OrderDate = _fixture.Today.ToString(), ShipmentDate = _fixture.Today.AddDays(10).ToString(), OrderStatus = OrderStatuses.New }; //yes, yes, yes, yes

            var request_fail_1 = new OrderRequest { CustomerId = _fixture.CustomerId_Exists, OrderDate = "99-99-9999" }; //yes, no, (yes, yes)
            var request_fail_2 = new OrderRequest { CustomerId = _fixture.CustomerId_Exists, OrderDate = _fixture.Today.ToString(), ShipmentDate = _fixture.Today.AddDays(-10).ToString() }; //yes, yes, no, (yes)
            var request_fail_3 = new OrderRequest { CustomerId = _fixture.CustomerId_Exists, OrderDate = _fixture.Today.AddDays(-10).ToString(), ShipmentDate = _fixture.Today.ToString(), OrderStatus = "unexists" }; //yes, yes, yes, no
            var request_fail_4 = new OrderRequest { CustomerId = _fixture.Guid_Unexists, OrderDate = _fixture.Today.ToString(), ShipmentDate = _fixture.Today.AddDays(-10).ToString(), OrderStatus = OrderStatuses.New }; //no, yes, yes, yes

            //Act
            var update_fail_1 = await service.Update(_fixture.Guid_Unexists, request_fake_success_1);
            var update_fail_2 = await service.Update(_fixture.Guid_Unexists, request_fake_success_2);
            var update_fail_3 = await service.Update(_fixture.Guid_Unexists, request_fake_success_3);
            var update_fail_4 = await service.Update(_fixture.CustomerId_Exists, request_fail_1);
            var update_fail_5 = await service.Update(_fixture.CustomerId_Exists, request_fail_2);
            var update_fail_6 = await service.Update(_fixture.CustomerId_Exists, request_fail_3);
            var update_fail_7 = await service.Update(_fixture.CustomerId_Exists, request_fail_4);

            //Assert
            Assert.False(update_fail_1);
            Assert.False(update_fail_2);
            Assert.False(update_fail_3);
            Assert.False(update_fail_4);
            Assert.False(update_fail_5);
            Assert.False(update_fail_6);
            Assert.False(update_fail_7);
        }

        [Fact]
        public async Task Delete()
        {
            //Arrange
            var service = new OrderService(_mockRepository.Object);

            //Act
            var delete_success = await service.Delete(_fixture.CustomerId_Exists);

            var delete_fail_1 = await service.Delete(_fixture.CustomerId_Exists);
            var delete_fail_2 = await service.Delete(_fixture.Guid_Unexists);
            var delete_fail_3 = await service.Delete(Guid.Empty);

            //Assert
            _mockRepository.Verify(x => x.Delete(_fixture.CustomerId_Exists), Times.Exactly(2));

            Assert.True(delete_success);

            Assert.False(delete_fail_1);
            Assert.False(delete_fail_2);
            Assert.False(delete_fail_3);
        }

        [Fact]
        public async Task PlaceAnOrder()
        {
            //Arrange
            var service = new OrderService(_mockRepository.Object);

            //Act
            var placeAnOrder_success = await service.PlaceAnOrder(_fixture.OrderId_Exists);

            var placeAnOrder_fail_1 = await service.PlaceAnOrder(_fixture.Guid_Unexists);
            var placeAnOrder_fail_2 = await service.PlaceAnOrder(Guid.Empty);

            //Assert
            Assert.True(placeAnOrder_success);

            Assert.False(placeAnOrder_fail_1);
            Assert.False(placeAnOrder_fail_2);
        }

        [Fact]
        public async Task GetBasketOrder()
        {
            //Arrange
            var service = new OrderService(_mockRepository.Object);

            //Act
            var placeAnOrder_fail_1 = await service.GetBasketOrder(Guid.Empty);
            var placeAnOrder_fail_2 = await service.GetBasketOrder(_fixture.Guid_Unexists);

            var placeAnOrder_success = await service.GetBasketOrder(_fixture.CustomerId_Exists);

            //Assert
            Assert.Null(placeAnOrder_fail_1);
            Assert.Null(placeAnOrder_fail_2);

            Assert.NotNull(placeAnOrder_success);
            Assert.Equal(_fixture.CustomerId_Exists, placeAnOrder_success.CustomerId);
        }

        [Fact]
        public async Task GetOneByCriteria_Success()
        {
            //Arrange
            var service = new OrderService(_mockRepository.Object);

            var criteria_success_1 = new OrderFilterCriteria();
            var criteria_success_2 = new OrderFilterCriteria
            {
                Id = _fixture.OrderId_Exists,
                CustomerId = _fixture.CustomerId_Exists,
                OrderNumber = 1,
                OrderStatus = OrderStatuses.Basket
            };

            //Act
            var getOneByCriteria_success_1 = await service.GetOneByCriteria(criteria_success_1);
            var getOneByCriteria_success_2 = await service.GetOneByCriteria(criteria_success_2);

            //Assert
            Assert.NotNull(getOneByCriteria_success_1);
            Assert.NotNull(getOneByCriteria_success_2);

            Assert.Equal(_fixture.OrderId_Exists, getOneByCriteria_success_1.Id);
            Assert.Equal(_fixture.CustomerId_Exists, getOneByCriteria_success_1.CustomerId);

            Assert.Equal(_fixture.OrderId_Exists, getOneByCriteria_success_2.Id);
            Assert.Equal(_fixture.CustomerId_Exists, getOneByCriteria_success_2.CustomerId);
        }

        [Fact]
        public async Task GetOneByCriteria_Fail()
        {
            //Arrange
            var service = new OrderService(_mockRepository.Object);

            var criteria_fail_1 = new OrderFilterCriteria { Id = Guid.Empty, CustomerId = _fixture.CustomerId_Exists, OrderNumber = 1, OrderStatus = null }; //no, yes, yes, yes
            var criteria_fail_2 = new OrderFilterCriteria { Id = _fixture.OrderId_Exists, CustomerId = Guid.Empty, OrderNumber = null, OrderStatus = OrderStatuses.New }; //yes, no, yes, yes
            var criteria_fail_3 = new OrderFilterCriteria { Id = _fixture.OrderId_Exists, CustomerId = _fixture.CustomerId_Exists, OrderNumber = -10, OrderStatus = OrderStatuses.Basket }; //yes, yes, no, yes
            var criteria_fail_4 = new OrderFilterCriteria { Id = _fixture.OrderId_Exists, CustomerId = _fixture.CustomerId_Exists, OrderNumber = 1, OrderStatus = OrderStatuses.NotExists }; //yes, yes, yes, no
            var criteria_fail_5 = new OrderFilterCriteria { Id = _fixture.Guid_Unexists, CustomerId = Guid.Empty, OrderNumber = -100, OrderStatus = OrderStatuses.NotExists }; //no, no, no, no

            //Act
            var getOneByCriteria_fail_1 = await service.GetOneByCriteria(criteria_fail_1);
            var getOneByCriteria_fail_2 = await service.GetOneByCriteria(criteria_fail_2);
            var getOneByCriteria_fail_3 = await service.GetOneByCriteria(criteria_fail_3);
            var getOneByCriteria_fail_4 = await service.GetOneByCriteria(criteria_fail_4);
            var getOneByCriteria_fail_5 = await service.GetOneByCriteria(criteria_fail_5);

            //Assert
            Assert.Null(getOneByCriteria_fail_1);
            Assert.Null(getOneByCriteria_fail_2);
            Assert.Null(getOneByCriteria_fail_3);
            Assert.Null(getOneByCriteria_fail_4);
            Assert.Null(getOneByCriteria_fail_5);
        }

        [Fact]
        public async Task GetPage_Success()
        {
            //Arrange
            var service = new OrderService(_mockRepository.Object);
            
            var pages1 = new PageInfo(1, 12);
            var pages2 = new PageInfo(1, 20);
            var pages3 = new PageInfo(2, 3);

            //Act
            var success_1 = await service.GetPage(pages1); //yes, yes
            var success_2 = await service.GetPage(pages2); //yes, yes
            var success_3 = await service.GetPage(pages3); //yes, yes

            //Assert
            Assert.NotNull(success_1);
            Assert.NotNull(success_2);
            Assert.NotNull(success_3);

            Assert.Equal(1, success_1.TotalCount);
            Assert.Equal(1, success_2.TotalCount);
            Assert.Equal(1, success_3.TotalCount);

            Assert.Single(success_1.Responses);
            Assert.Single(success_2.Responses);
            Assert.Empty(success_3.Responses);
        }

        [Fact]
        public async Task GetPage_Fail()
        {
            //Arrange
            var service = new OrderService(_mockRepository.Object);
            
            var pages1 = new PageInfo(0, 0);
            var pages2 = new PageInfo(-1, 1);
            var pages3 = new PageInfo(1, -1);
            var pages4 = new PageInfo(1000, 1000);
            var pages5 = new PageInfo(-1000, -1000);
            var pages6 = new PageInfo(1, 1000);

            //Act
            var fail_1 = await service.GetPage(pages1); //no, no
            var fail_2 = await service.GetPage(pages2); //no, yes
            var fail_3 = await service.GetPage(pages3);  //yes, no
            var fail_4 = await service.GetPage(pages4); //yes, no
            var fail_5 = await service.GetPage(pages5); //no, no
            var fail_6 = await service.GetPage(pages6); //yes, no

            //Assert
            Assert.NotNull(fail_1);
            Assert.NotNull(fail_2);
            Assert.NotNull(fail_3);
            Assert.NotNull(fail_4);
            Assert.NotNull(fail_5);
            Assert.NotNull(fail_6);

            Assert.Equal(0, fail_1.TotalCount);
            Assert.Equal(0, fail_2.TotalCount);
            Assert.Equal(0, fail_3.TotalCount);
            Assert.Equal(0, fail_4.TotalCount);
            Assert.Equal(0, fail_5.TotalCount);
            Assert.Equal(0, fail_6.TotalCount);

            Assert.Empty(fail_1.Responses);
            Assert.Empty(fail_2.Responses);
            Assert.Empty(fail_3.Responses);
            Assert.Empty(fail_4.Responses);
            Assert.Empty(fail_5.Responses);
            Assert.Empty(fail_6.Responses);
        }

        [Fact]
        public async Task GetPageByCriteria_Success()
        {
            //Arrange
            var service = new OrderService(_mockRepository.Object);

            var criteria_success_1 = new OrderFilterCriteria();
            var criteria_success_2 = new OrderFilterCriteria
            {
                Id = _fixture.OrderId_Exists,
                CustomerId = _fixture.CustomerId_Exists,
                OrderNumber = 1,
                OrderStatus = OrderStatuses.Basket
            };

            var pages1 = new PageInfo(1, 12);
            var pages2 = new PageInfo(1, 20);
            var pages3 = new PageInfo(2, 3);

            //Act
            var success_1 = await service.GetPageByCriteria(criteria_success_1, pages1); //yes, yes, yes
            var success_2 = await service.GetPageByCriteria(criteria_success_2, pages1); //yes, yes, yes

            var success_3 = await service.GetPageByCriteria(criteria_success_1, pages2); //yes, yes, yes
            var success_4 = await service.GetPageByCriteria(criteria_success_2, pages2); //yes, yes, yes

            var success_5 = await service.GetPageByCriteria(criteria_success_1, pages3); //yes, yes, yes
            var success_6 = await service.GetPageByCriteria(criteria_success_2, pages3); //yes, yes, yes

            //Assert
            Assert.NotNull(success_1);
            Assert.NotNull(success_2);
            Assert.NotNull(success_3);
            Assert.NotNull(success_4);
            Assert.NotNull(success_5);
            Assert.NotNull(success_6);

            Assert.Equal(1, success_1.TotalCount);
            Assert.Equal(1, success_2.TotalCount);
            Assert.Equal(1, success_3.TotalCount);
            Assert.Equal(1, success_4.TotalCount);
            Assert.Equal(1, success_5.TotalCount);
            Assert.Equal(1, success_6.TotalCount);

            Assert.Single(success_1.Responses);
            Assert.Single(success_2.Responses);
            Assert.Single(success_3.Responses);
            Assert.Single(success_4.Responses);

            Assert.Empty(success_5.Responses);
            Assert.Empty(success_6.Responses);
        }

        [Fact]
        public async Task GetPageByCriteria_Fail()
        {
            //Arrange
            var service = new OrderService(_mockRepository.Object);

            var criteria_fake_success_1 = new OrderFilterCriteria();
            var criteria_fake_success_2 = new OrderFilterCriteria
            {
                Id = _fixture.OrderId_Exists,
                CustomerId = _fixture.CustomerId_Exists,
                OrderNumber = 1,
                OrderStatus = OrderStatuses.Basket
            };

            var criteria_fail_1 = new OrderFilterCriteria { Id = Guid.Empty, CustomerId = _fixture.CustomerId_Exists, OrderNumber = 1, OrderStatus = null }; //no, yes, yes, yes
            var criteria_fail_2 = new OrderFilterCriteria { Id = _fixture.OrderId_Exists, CustomerId = Guid.Empty, OrderNumber = null, OrderStatus = OrderStatuses.New }; //yes, no, yes, yes
            var criteria_fail_3 = new OrderFilterCriteria { Id = _fixture.OrderId_Exists, CustomerId = _fixture.CustomerId_Exists, OrderNumber = -10, OrderStatus = OrderStatuses.Basket }; //yes, yes, no, yes
            var criteria_fail_4 = new OrderFilterCriteria { Id = _fixture.OrderId_Exists, CustomerId = _fixture.CustomerId_Exists, OrderNumber = 1, OrderStatus = OrderStatuses.NotExists }; //yes, yes, yes, no
            var criteria_fail_5 = new OrderFilterCriteria { Id = _fixture.Guid_Unexists, CustomerId = Guid.Empty, OrderNumber = -100, OrderStatus = OrderStatuses.NotExists }; //no, no, no, no

            var pages1 = new PageInfo(1, 12);
            var pages2 = new PageInfo(2, 3);
            var pages3 = new PageInfo(1, 20);
            
            var pages4 = new PageInfo(0, 0);
            var pages5 = new PageInfo(-1, 1);
            var pages6 = new PageInfo(1, -1);
            var pages7 = new PageInfo(1000, 1000);
            var pages8 = new PageInfo(-1000, -1000);
            var pages9 = new PageInfo(1, 1000);

            //Act
            var fake_success_1 = await service.GetPageByCriteria(criteria_fail_1, pages1); //no, yes, yes
            var fake_success_2 = await service.GetPageByCriteria(criteria_fail_2, pages2); //no, yes, yes
            var fake_success_3 = await service.GetPageByCriteria(criteria_fail_3, pages3); //no, yes, yes

            var fail_1 = await service.GetPageByCriteria(criteria_fake_success_1, pages4); //yes, no, no
            var fail_2 = await service.GetPageByCriteria(criteria_fake_success_1, pages5); //yes, no, yes
            var fail_3 = await service.GetPageByCriteria(criteria_fake_success_1, pages6);  //yes, yes, no
            var fail_4 = await service.GetPageByCriteria(criteria_fake_success_1, pages7); //yes, yes, no
            var fail_5 = await service.GetPageByCriteria(criteria_fake_success_1, pages8); //yes, no, no
            var fail_6 = await service.GetPageByCriteria(criteria_fake_success_1, pages9); //yes, yes, no

            var fail_7 = await service.GetPageByCriteria(criteria_fake_success_2, pages4); //yes, no, no
            var fail_8 = await service.GetPageByCriteria(criteria_fake_success_2, pages5); //yes, no, yes
            var fail_9 = await service.GetPageByCriteria(criteria_fake_success_2, pages6);  //yes, yes, no
            var fail_10 = await service.GetPageByCriteria(criteria_fake_success_2, pages7); //yes, yes, no
            var fail_11 = await service.GetPageByCriteria(criteria_fake_success_2, pages8); //yes, no, no
            var fail_12 = await service.GetPageByCriteria(criteria_fake_success_2, pages9); //yes, yes, no

            //Assert
            Assert.NotNull(fake_success_1);
            Assert.NotNull(fake_success_2);
            Assert.NotNull(fake_success_3);

            Assert.Empty(fake_success_1.Responses);
            Assert.Empty(fake_success_2.Responses);
            Assert.Empty(fake_success_3.Responses);

            Assert.Equal(0, fake_success_1.TotalCount);
            Assert.Equal(0, fake_success_2.TotalCount);
            Assert.Equal(0, fake_success_3.TotalCount);

            Assert.NotNull(fail_1);
            Assert.NotNull(fail_2);
            Assert.NotNull(fail_3);
            Assert.NotNull(fail_4);
            Assert.NotNull(fail_5);
            Assert.NotNull(fail_6);
            Assert.NotNull(fail_7);
            Assert.NotNull(fail_8);
            Assert.NotNull(fail_9);
            Assert.NotNull(fail_10);
            Assert.NotNull(fail_11);
            Assert.NotNull(fail_12);

            Assert.Empty(fail_1.Responses);
            Assert.Empty(fail_2.Responses);
            Assert.Empty(fail_3.Responses);
            Assert.Empty(fail_4.Responses);
            Assert.Empty(fail_5.Responses);
            Assert.Empty(fail_6.Responses);
            Assert.Empty(fail_7.Responses);
            Assert.Empty(fail_8.Responses);
            Assert.Empty(fail_9.Responses);
            Assert.Empty(fail_10.Responses);
            Assert.Empty(fail_11.Responses);
            Assert.Empty(fail_12.Responses);

            Assert.Equal(0, fail_1.TotalCount);
            Assert.Equal(0, fail_2.TotalCount);
            Assert.Equal(0, fail_3.TotalCount);
            Assert.Equal(0, fail_4.TotalCount);
            Assert.Equal(0, fail_5.TotalCount);
            Assert.Equal(0, fail_6.TotalCount);
            Assert.Equal(0, fail_7.TotalCount);
            Assert.Equal(0, fail_8.TotalCount);
            Assert.Equal(0, fail_9.TotalCount);
            Assert.Equal(0, fail_10.TotalCount);
            Assert.Equal(0, fail_11.TotalCount);
            Assert.Equal(0, fail_12.TotalCount);
        }
    }
}
