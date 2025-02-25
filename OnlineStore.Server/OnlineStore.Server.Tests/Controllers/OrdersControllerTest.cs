using OnlineStore.Server.Controllers;

namespace OnlineStore.Server.Tests.Controllers
{
    public class OrdersControllerTest
    {
        [Fact]
        public void CheckControllerName()
        {
            // проверка имени
            Assert.Equal("OrdersController", typeof(OrdersController).Name);
        }

        [Fact]
        public void CheckMethodsNames()
        {
            // проверка имен
            Assert.NotNull(typeof(OrdersController).GetMethod("GetPageOfOrders"));
            Assert.NotNull(typeof(OrdersController).GetMethod("GetPageOfOrdersByCustomerId"));
            Assert.NotNull(typeof(OrdersController).GetMethod("GetPageOfOrdersByStatus"));
            Assert.NotNull(typeof(OrdersController).GetMethod("GetOrderByNumber"));
            Assert.NotNull(typeof(OrdersController).GetMethod("GetBasketOrder"));
            Assert.NotNull(typeof(OrdersController).GetMethod("PlaceAnOrder"));
            Assert.NotNull(typeof(OrdersController).GetMethod("CreateOrder"));
            Assert.NotNull(typeof(OrdersController).GetMethod("UpdateOrder"));
            Assert.NotNull(typeof(OrdersController).GetMethod("DeleteOrder"));
        }
    }
}
