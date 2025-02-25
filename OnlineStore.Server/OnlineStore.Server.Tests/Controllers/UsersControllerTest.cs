using OnlineStore.Server.Controllers;

namespace OnlineStore.Server.Tests.Controllers
{
    public class UsersControllerTest
    {
        [Fact]
        public void CheckControllerName()
        {
            // проверка имени
            Assert.Equal("UsersController", typeof(UsersController).Name);
        }

        [Fact]
        public void CheckMethodsNames()
        {
            // проверка имен
            Assert.NotNull(typeof(UsersController).GetMethod("GetPageOfUsersInfo"));
            Assert.NotNull(typeof(UsersController).GetMethod("Authenticate"));
            Assert.NotNull(typeof(UsersController).GetMethod("RegisterUser"));
            Assert.NotNull(typeof(UsersController).GetMethod("RegisterManager"));
            Assert.NotNull(typeof(UsersController).GetMethod("UpdateUser"));
            Assert.NotNull(typeof(UsersController).GetMethod("DeleteUser"));
        }
    }
}
