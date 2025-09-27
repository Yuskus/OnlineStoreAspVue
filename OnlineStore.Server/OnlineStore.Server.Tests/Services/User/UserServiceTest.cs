using Moq;
using OnlineStore.Server.Database.Entities;
using OnlineStore.Server.DTO.User;
using OnlineStore.Server.Repositories.User;
using OnlineStore.Server.Services.User;

namespace OnlineStore.Server.Tests.Services.User
{
    [Collection("UserServiceCollection")]
    public class UserServiceTest : IClassFixture<UserServiceFixture>
    {
        private readonly UserServiceFixture _fixture;
        private readonly Mock<IUserRepository> _mockRepository;

        public UserServiceTest(UserServiceFixture fixture)
        {
            _fixture = fixture;
            _mockRepository = _fixture.CreateMockRepository();
        }

        [Fact]
        public async Task Auth_Success()
        {
            //Arrange
            var service = new UserService(_mockRepository.Object);

            // exists and ok password
            var request_success_1 = new UserCredentialsRequest { Username = _fixture.Username_Exists[0], Password = "12345678" };
            var request_success_2 = new UserCredentialsRequest { Username = _fixture.Username_Exists[1], Password = "12345678" };

            //Act
            var auth_success_1 = await service.Authenticate(request_success_1);
            var auth_success_2 = await service.Authenticate(request_success_2);

            //Assert
            Assert.NotNull(auth_success_1);
            Assert.NotNull(auth_success_2);

            Assert.Equal(_fixture.Username_Exists[0], auth_success_1.Username);
            Assert.Equal(_fixture.Username_Exists[1], auth_success_2.Username);

            Assert.Equal(_fixture.Token_Exists, auth_success_1.Token);
            Assert.Equal(_fixture.Token_Exists, auth_success_2.Token);
        }

        [Fact]
        public async Task Auth_Fail()
        {
            //Arrange
            var service = new UserService(_mockRepository.Object);

            // exists and wrong password
            var request_fail_1 = new UserCredentialsRequest { Username = _fixture.Username_Exists[0], Password = null! };
            var request_fail_2 = new UserCredentialsRequest { Username = _fixture.Username_Exists[1], Password = "" };
            var request_fail_3 = new UserCredentialsRequest { Username = _fixture.Username_Exists[1], Password = "00000" };
            var request_fail_4 = new UserCredentialsRequest { Username = _fixture.Username_Exists[1], Password = new string(' ', 8) };
            var request_fail_5 = new UserCredentialsRequest { Username = _fixture.Username_Exists[0], Password = new string('a', 100) };

            // unexists and ok password
            var request_fail_6 = new UserCredentialsRequest { Username = _fixture.Username_Unexists, Password = "12345678" };

            // unexists and wrong password
            var request_fail_7 = new UserCredentialsRequest { Username = _fixture.Username_Unexists, Password = null! };
            var request_fail_8 = new UserCredentialsRequest { Username = _fixture.Username_Unexists, Password = "" };
            var request_fail_9 = new UserCredentialsRequest { Username = _fixture.Username_Unexists, Password = "00000" };
            var request_fail_10 = new UserCredentialsRequest { Username = _fixture.Username_Unexists, Password = new string(' ', 8) };
            var request_fail_11 = new UserCredentialsRequest { Username = _fixture.Username_Unexists, Password = new string('a', 100) };

            // unexists and wrong username
            var request_fail_12 = new UserCredentialsRequest { Username = null!, Password = "12345678" };
            var request_fail_13 = new UserCredentialsRequest { Username = "", Password = "12345678" };
            var request_fail_14 = new UserCredentialsRequest { Username = new string(' ', 8), Password = "12345678" };
            var request_fail_15 = new UserCredentialsRequest { Username = new string('a', 100), Password = "12345678" };

            //Act
            var auth_fail_1 = await service.Authenticate(request_fail_1);
            var auth_fail_2 = await service.Authenticate(request_fail_2);
            var auth_fail_3 = await service.Authenticate(request_fail_3);
            var auth_fail_4 = await service.Authenticate(request_fail_4);
            var auth_fail_5 = await service.Authenticate(request_fail_5);

            var auth_fail_6 = await service.Authenticate(request_fail_6);

            var auth_fail_7 = await service.Authenticate(request_fail_7);
            var auth_fail_8 = await service.Authenticate(request_fail_8);
            var auth_fail_9 = await service.Authenticate(request_fail_9);
            var auth_fail_10 = await service.Authenticate(request_fail_10);
            var auth_fail_11 = await service.Authenticate(request_fail_11);

            var auth_fail_12 = await service.Authenticate(request_fail_12);
            var auth_fail_13 = await service.Authenticate(request_fail_13);
            var auth_fail_14 = await service.Authenticate(request_fail_14);
            var auth_fail_15 = await service.Authenticate(request_fail_15);

            //Assert
            Assert.Null(auth_fail_1);
            Assert.Null(auth_fail_2);
            Assert.Null(auth_fail_3);
            Assert.Null(auth_fail_4);
            Assert.Null(auth_fail_5);
            Assert.Null(auth_fail_6);
            Assert.Null(auth_fail_7);
            Assert.Null(auth_fail_8);
            Assert.Null(auth_fail_9);
            Assert.Null(auth_fail_10);
            Assert.Null(auth_fail_11);
            Assert.Null(auth_fail_12);
            Assert.Null(auth_fail_13);
            Assert.Null(auth_fail_14);
            Assert.Null(auth_fail_15);
        }

        [Fact]
        public async Task Update_Success()
        {
            //Arrange
            var service = new UserService(_mockRepository.Object);

            var request_success_1 = new UserRequest { Username = _fixture.Username_Exists[0], Role = UserRole.User };
            var request_success_2 = new UserRequest { Username = _fixture.Username_Exists[1], Role = UserRole.Manager };

            //Act
            var update_success_1 = await service.Update(_fixture.Username_Exists[0], request_success_1);
            var update_success_2 = await service.Update(_fixture.Username_Exists[1], request_success_2);
            var update_success_3 = await service.Update(_fixture.Username_Exists[0], request_success_2);
            var update_success_4 = await service.Update(_fixture.Username_Exists[1], request_success_1);

            //Assert
            Assert.True(update_success_1);
            Assert.True(update_success_2);
            Assert.True(update_success_3);
            Assert.True(update_success_4);
        }

        [Fact]
        public async Task Update_Fail()
        {
            //Arrange
            var service = new UserService(_mockRepository.Object);

            var request_fake_success_1 = new UserRequest { Username = _fixture.Username_Exists[0], Role = UserRole.User };
            var request_fake_success_2 = new UserRequest { Username = _fixture.Username_Exists[1], Role = UserRole.Manager };

            var request_fail_1 = new UserRequest { Username = _fixture.Username_Unexists, Role = UserRole.User };
            var request_fail_2 = new UserRequest { Username = _fixture.Username_Unexists, Role = UserRole.Manager };
            var request_fail_3 = new UserRequest { Username = null!, Role = It.IsAny<UserRole>() };
            var request_fail_4 = new UserRequest { Username = "", Role = It.IsAny<UserRole>() };
            var request_fail_5 = new UserRequest { Username = new string(' ', 8), Role = It.IsAny<UserRole>() };
            var request_fail_6 = new UserRequest { Username = new string('a', 100), Role = It.IsAny<UserRole>() };

            //Act
            var update_fail_1 = await service.Update(_fixture.Username_Unexists, request_fake_success_1); //ny
            var update_fail_2 = await service.Update(_fixture.Username_Unexists, request_fake_success_2); //ny

            var update_fail_3 = await service.Update(_fixture.Username_Exists[0], request_fail_1); //yn
            var update_fail_4 = await service.Update(_fixture.Username_Exists[1], request_fail_2); //yn
            var update_fail_5 = await service.Update(_fixture.Username_Exists[0], request_fail_3); //yn
            var update_fail_6 = await service.Update(_fixture.Username_Exists[1], request_fail_4); //yn
            var update_fail_7 = await service.Update(_fixture.Username_Exists[0], request_fail_5); //yn
            var update_fail_8 = await service.Update(_fixture.Username_Exists[1], request_fail_6); //yn

            //Assert
            Assert.False(update_fail_1);
            Assert.False(update_fail_2);

            Assert.False(update_fail_3);
            Assert.False(update_fail_4);
            Assert.False(update_fail_5);
            Assert.False(update_fail_6);
            Assert.False(update_fail_7);
            Assert.False(update_fail_8);
        }

        [Fact]
        public async Task Delete()
        {
            //Arrange
            var service = new UserService(_mockRepository.Object);

            //Act
            var delete_success_1 = await service.Delete(_fixture.Username_Exists[0]);
            var delete_success_2 = await service.Delete(_fixture.Username_Exists[1]);

            var delete_fail_1 = await service.Delete(_fixture.Username_Exists[0]);
            var delete_fail_2 = await service.Delete(_fixture.Username_Exists[1]);

            var delete_fail_3 = await service.Delete(_fixture.Username_Unexists);
            var delete_fail_4 = await service.Delete(null!);
            var delete_fail_5 = await service.Delete("");
            var delete_fail_6 = await service.Delete(new string(' ', 12));
            var delete_fail_7 = await service.Delete(new string('a', 3));
            var delete_fail_8 = await service.Delete(new string('a', 100));

            //Assert
            _mockRepository.Verify(x => x.Delete(_fixture.Username_Exists[0]), Times.Exactly(2));
            _mockRepository.Verify(x => x.Delete(_fixture.Username_Exists[1]), Times.Exactly(2));

            Assert.True(delete_success_1);
            Assert.True(delete_success_2);

            Assert.False(delete_fail_1);
            Assert.False(delete_fail_2);
            Assert.False(delete_fail_3);
            Assert.False(delete_fail_4);
            Assert.False(delete_fail_5);
            Assert.False(delete_fail_6);
            Assert.False(delete_fail_7);
            Assert.False(delete_fail_8);
        }

        [Fact]
        public async Task GetPage_Success()
        {
            //Arrange
            var service = new UserService(_mockRepository.Object);

            //Act
            var success_1 = await service.GetPage(1, 12); //yes, yes
            var success_2 = await service.GetPage(1, 20); //yes, yes
            var success_3 = await service.GetPage(2, 3); //yes, yes

            //Assert
            Assert.NotNull(success_1);
            Assert.NotNull(success_2);
            Assert.NotNull(success_3);

            Assert.Equal(2, success_1.TotalCount);
            Assert.Equal(2, success_2.TotalCount);
            Assert.Equal(2, success_3.TotalCount);

            Assert.Equal(2, success_1.Responses.Count());
            Assert.Equal(2, success_2.Responses.Count());

            Assert.Empty(success_3.Responses);
        }

        [Fact]
        public async Task GetPage_Fail()
        {
            //Arrange
            var service = new UserService(_mockRepository.Object);

            //Act
            var fail_1 = await service.GetPage(0, 0); //no, no
            var fail_2 = await service.GetPage(-1, 1); //no, yes
            var fail_3 = await service.GetPage(1, -1);  //yes, no
            var fail_4 = await service.GetPage(1000, 1000); //yes, no
            var fail_5 = await service.GetPage(-1000, -1000); //no, no
            var fail_6 = await service.GetPage(1, 1000); //yes, no

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
    }
}
