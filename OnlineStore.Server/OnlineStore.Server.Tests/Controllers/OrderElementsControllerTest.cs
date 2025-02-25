using OnlineStore.Server.Controllers;

namespace OnlineStore.Server.Tests.Controllers
{
    public class OrderElementsControllerTest
    {
        [Fact]
        public void CheckControllerName()
        {
            // проверка имени
            Assert.Equal("OrderElementsController", typeof(OrderElementsController).Name);
        }

        [Fact]
        public void CheckMethodsNames()
        {
            // проверка имен
            Assert.NotNull(typeof(OrderElementsController).GetMethod("GetOrderElementsByOrderId"));
            Assert.NotNull(typeof(OrderElementsController).GetMethod("CreateOrderElement"));
            Assert.NotNull(typeof(OrderElementsController).GetMethod("UpdateOrderElement"));
            Assert.NotNull(typeof(OrderElementsController).GetMethod("DeleteOrderElement"));
        }
    }
}
