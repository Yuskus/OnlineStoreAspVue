using OnlineStore.Server.Controllers;

namespace OnlineStore.Server.Tests.Controllers
{
    public class CustomersControllerTest
    {
        [Fact]
        public void CheckControllerName()
        {
            // проверка имени контроллера
            Assert.Equal("CustomersController", typeof(CustomersController).Name);
        }

        [Fact]
        public void CheckMethodsNames()
        {
            // проверка имен методов
            Assert.NotNull(typeof(CustomersController).GetMethod("GetPageOfCustomers"));
            Assert.NotNull(typeof(CustomersController).GetMethod("GetCustomerById"));
            Assert.NotNull(typeof(CustomersController).GetMethod("GetCustomerByCode"));
            Assert.NotNull(typeof(CustomersController).GetMethod("UpdateCustomer"));
        }
    }
}
