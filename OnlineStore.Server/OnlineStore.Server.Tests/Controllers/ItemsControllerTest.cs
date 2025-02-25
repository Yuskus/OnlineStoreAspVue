using OnlineStore.Server.Controllers;

namespace OnlineStore.Server.Tests.Controllers
{
    public class ItemsControllerTest
    {
        [Fact]
        public void CheckControllerName()
        {
            // проверка имени
            Assert.Equal("ItemsController", typeof(ItemsController).Name);
        }

        [Fact]
        public void CheckMethodsNames()
        {
            // проверка имен
            Assert.NotNull(typeof(ItemsController).GetMethod("GetPageOfItems"));
            Assert.NotNull(typeof(ItemsController).GetMethod("GetPageOfItemsByCategory"));
            Assert.NotNull(typeof(ItemsController).GetMethod("GetAllCategories"));
            Assert.NotNull(typeof(ItemsController).GetMethod("GetItemById"));
            Assert.NotNull(typeof(ItemsController).GetMethod("GetItemByCode"));
            Assert.NotNull(typeof(ItemsController).GetMethod("GetItemByName"));
            Assert.NotNull(typeof(ItemsController).GetMethod("CreateItem"));
            Assert.NotNull(typeof(ItemsController).GetMethod("UpdateItem"));
            Assert.NotNull(typeof(ItemsController).GetMethod("DeleteItem"));
        }
    }
}
