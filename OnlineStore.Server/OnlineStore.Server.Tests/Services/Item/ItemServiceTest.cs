using Moq;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Items;
using OnlineStore.Server.Repositories.Items;
using OnlineStore.Server.Services.Items;

namespace OnlineStore.Server.Tests.Services.Item
{
    [Collection("ItemServiceCollection")]
    public class ItemServiceTest : IClassFixture<ItemServiceFixture>
    {
        private readonly ItemServiceFixture _fixture;
        private readonly Mock<IItemRepository> _mockRepository;
        public ItemServiceTest(ItemServiceFixture fixture)
        {
            _fixture = fixture;
            _mockRepository = _fixture.CreateMockRepository();
        }

        [Fact]
        public async Task Create_Success()
        {
            //Arrange
            var service = new ItemService(_mockRepository.Object);

            var request_success_1 = new ItemRequest { Code = _fixture.ItemCode_Exists, Name = _fixture.ItemName_Exists }; //yes, yes, (yes, yes)
            var request_success_2 = new ItemRequest { Code = _fixture.ItemCode_Exists, Name = _fixture.ItemName_Exists, Price = _fixture.ItemPrice_Exists, Category = _fixture.ItemCode_Exists }; //yes, yes, yes, yes

            //Act
            var update_success_1 = await service.Create(request_success_1);
            var update_success_2 = await service.Create(request_success_2);

            //Assert
            Assert.NotNull(update_success_1);
            Assert.NotNull(update_success_2);

            Assert.NotEqual(Guid.Empty, update_success_1);
            Assert.NotEqual(Guid.Empty, update_success_2);

            Assert.Equal(update_success_1, update_success_2);

        }

        [Fact]
        public async Task Create_Fail()
        {
            //Arrange
            var service = new ItemService(_mockRepository.Object);

            var request_fail_1 = new ItemRequest { Code = _fixture.ItemCode_Exists, Name = _fixture.ItemName_Exists, Price = -10, Category = _fixture.ItemCategory_Exists }; //yes, yes, no, yes
            var request_fail_2 = new ItemRequest { Code = _fixture.ItemCode_Exists, Name = "", Price = _fixture.ItemPrice_Exists, Category = "                  " }; //yes, no, yes, no
            var request_fail_3 = new ItemRequest { Code = _fixture.ItemCode_Exists, Name = "              ", Price = null, Category = _fixture.ItemCategory_Exists }; //yes, no, yes, yes
            var request_fail_4 = new ItemRequest { Code = "11111111YU11", Name = _fixture.ItemName_Exists, Price = -10, Category = "" }; //no, yes, no, no
            var request_fail_5 = new ItemRequest { Code = _fixture.ItemCode_Exists + " ", Name = _fixture.ItemName_Exists, Price = _fixture.ItemPrice_Exists, Category = _fixture.ItemCode_Exists };//no, yes, yes, yes

            //Act
            var create_fail_1 = await service.Create(request_fail_1);
            var create_fail_2 = await service.Create(request_fail_2);
            var create_fail_3 = await service.Create(request_fail_3);
            var create_fail_4 = await service.Create(request_fail_4);
            var create_fail_5 = await service.Create(request_fail_5);

            //Assert
            Assert.Null(create_fail_1);
            Assert.Null(create_fail_2);
            Assert.Null(create_fail_3);
            Assert.Null(create_fail_4);
            Assert.Null(create_fail_5);
        }

        [Fact]
        public async Task Update_Success()
        {
            //Arrange
            var service = new ItemService(_mockRepository.Object);

            var request_success_1 = new ItemRequest { Code = _fixture.ItemCode_Exists, Name = _fixture.ItemName_Exists }; //yes, yes, (yes, yes)
            var request_success_2 = new ItemRequest { Code = _fixture.ItemCode_Exists, Name = _fixture.ItemName_Exists, Price = _fixture.ItemPrice_Exists, Category = _fixture.ItemCode_Exists }; //yes, yes, yes, yes

            //Act
            var update_success_1 = await service.Update(_fixture.ItemGuid_Exists, request_success_1);
            var update_success_2 = await service.Update(_fixture.ItemGuid_Exists, request_success_2);

            //Assert
            Assert.True(update_success_1);
            Assert.True(update_success_2);
        }

        [Fact]
        public async Task Update_Fail()
        {
            //Arrange
            var service = new ItemService(_mockRepository.Object);

            var request_fail_1 = new ItemRequest { Code = _fixture.ItemCode_Exists, Name = _fixture.ItemName_Exists, Price = -10, Category = _fixture.ItemCategory_Exists }; //yes, yes, no, yes
            var request_fail_2 = new ItemRequest { Code = _fixture.ItemCode_Exists, Name = "", Price = _fixture.ItemPrice_Exists, Category = "                  " }; //yes, no, yes, no
            var request_fail_3 = new ItemRequest { Code = _fixture.ItemCode_Exists, Name = "              ", Price = null, Category = _fixture.ItemCategory_Exists }; //yes, no, yes, yes
            var request_fail_4 = new ItemRequest { Code = "11111111YU11", Name = _fixture.ItemName_Exists, Price = -10, Category = "" }; //no, yes, no, no
            var request_fail_5 = new ItemRequest { Code = _fixture.ItemCode_Exists + " ", Name = _fixture.ItemName_Exists, Price = _fixture.ItemPrice_Exists, Category = _fixture.ItemCode_Exists };//no, yes, yes, yes
            
            var request_fake_success_1 = new ItemRequest { Code = _fixture.ItemCode_Exists, Name = _fixture.ItemName_Exists }; //yes, yes, (yes, yes)
            var request_fake_success_2 = new ItemRequest { Code = _fixture.ItemCode_Exists, Name = _fixture.ItemName_Exists, Price = _fixture.ItemPrice_Exists, Category = _fixture.ItemCode_Exists }; //yes, yes, yes, yes
            
            //Act
            var update_fail_1 = await service.Update(Guid.Empty, request_fake_success_1);
            var update_fail_2 = await service.Update(_fixture.Guid_Unexists, request_fake_success_2);
            var update_fail_3 = await service.Update(_fixture.ItemGuid_Exists, request_fail_1);
            var update_fail_4 = await service.Update(_fixture.ItemGuid_Exists, request_fail_2);
            var update_fail_5 = await service.Update(_fixture.ItemGuid_Exists, request_fail_3);
            var update_fail_6 = await service.Update(_fixture.ItemGuid_Exists, request_fail_4);
            var update_fail_7 = await service.Update(_fixture.ItemGuid_Exists, request_fail_5);

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
            var service = new ItemService(_mockRepository.Object);

            //Act
            var delete_Success = await service.Delete(_fixture.ItemGuid_Exists);

            var delete_Fail_1 = await service.Delete(_fixture.ItemGuid_Exists);
            var delete_Fail_2 = await service.Delete(_fixture.Guid_Unexists);
            var delete_Fail_3 = await service.Delete(Guid.Empty);

            //Assert
            _mockRepository.Verify(x => x.Delete(_fixture.ItemGuid_Exists), Times.Exactly(2));

            Assert.True(delete_Success);

            Assert.False(delete_Fail_1);
            Assert.False(delete_Fail_2);
            Assert.False(delete_Fail_3);
        }

        [Fact]
        public async Task GetPage_Success()
        {
            //Arrange
            var service = new ItemService(_mockRepository.Object);

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
            var service = new ItemService(_mockRepository.Object);

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
            var service = new ItemService(_mockRepository.Object);

            var criteria_success_1 = new ItemFilterCriteria { Name = _fixture.ItemName_Exists };
            var criteria_success_2 = new ItemFilterCriteria { Category = _fixture.ItemCategory_Exists };
            var criteria_success_3 = new ItemFilterCriteria();
            
            var pages1 = new PageInfo(1, 12);
            var pages2 = new PageInfo(1, 20);
            var pages3 = new PageInfo(2, 3);

            //Act
            var success_1 = await service.GetPageByCriteria(criteria_success_1, pages1); //yes, yes, yes
            var success_2 = await service.GetPageByCriteria(criteria_success_2, pages2); //yes, yes, yes
            var success_3 = await service.GetPageByCriteria(criteria_success_3, pages3); //yes, yes, yes

            //Assert
            Assert.NotNull(success_1);
            Assert.NotNull(success_2);
            Assert.NotNull(success_3);

            Assert.Equal(1, success_1.TotalCount);
            Assert.Equal(1, success_2.TotalCount);
            Assert.Equal(1, success_3.TotalCount);

            Assert.Single(success_1.Responses);
            Assert.Single(success_2.Responses);
            Assert.Empty(success_3.Responses); //2-я страница не существует: responses пуст, а total count - 1 элемент

            Assert.Equal(_fixture.ItemGuid_Exists, success_1.Responses.First().Id);
            Assert.Equal(_fixture.ItemGuid_Exists, success_2.Responses.First().Id);
        }

        [Fact]
        public async Task GetPageByCriteria_Fail()
        {
            //Arrange
            var service = new ItemService(_mockRepository.Object);

            var pages1 = new PageInfo(0, 0);
            var pages2 = new PageInfo(-1, 1);
            var pages3 = new PageInfo(1, -1);
            var pages4 = new PageInfo(1000, 1000);
            var pages5 = new PageInfo(-1000, -1000);
            var pages6 = new PageInfo(1, 1000);

            var pages7 = new PageInfo(1, 12);
            var pages8 = new PageInfo(2, 3);
            var pages9 = new PageInfo(1, 20);

            var criteria_fail_1 = new ItemFilterCriteria { Category = "Unexists" };

            var criteria_fake_success_1 = new ItemFilterCriteria();
            var criteria_fake_success_2 = new ItemFilterCriteria { Name = _fixture.ItemName_Exists };
            var criteria_fake_success_3 = new ItemFilterCriteria { Category = _fixture.ItemCategory_Exists };
            var criteria_fake_success_4 = new ItemFilterCriteria { Name = _fixture.ItemName_Exists, Category = _fixture.ItemCategory_Exists };
            
            //Act
            var fake_success_1 = await service.GetPageByCriteria(criteria_fail_1, pages7); //no, yes, yes
            var fake_success_2 = await service.GetPageByCriteria(criteria_fail_1, pages8); //no, yes, yes
            var fake_success_3 = await service.GetPageByCriteria(criteria_fail_1, pages9); //no, yes, yes

            var fail_1 = await service.GetPageByCriteria(criteria_fake_success_1, pages1); //yes, no, no
            var fail_2 = await service.GetPageByCriteria(criteria_fake_success_2, pages2); //yes, no, no
            var fail_3 = await service.GetPageByCriteria(criteria_fake_success_3, pages3);  //yes, yes, no
            var fail_4 = await service.GetPageByCriteria(criteria_fake_success_4, pages4);  //yes, yes, no
            var fail_5 = await service.GetPageByCriteria(criteria_fake_success_2, pages5); //yes, no, no
            var fail_6 = await service.GetPageByCriteria(criteria_fake_success_4, pages6); //yes, yes, no

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

            Assert.Empty(fail_1.Responses);
            Assert.Empty(fail_2.Responses);
            Assert.Empty(fail_3.Responses);
            Assert.Empty(fail_4.Responses);
            Assert.Empty(fail_5.Responses);
            Assert.Empty(fail_6.Responses);

            Assert.Equal(0, fail_1.TotalCount);
            Assert.Equal(0, fail_2.TotalCount);
            Assert.Equal(0, fail_3.TotalCount);
            Assert.Equal(0, fail_4.TotalCount);
            Assert.Equal(0, fail_5.TotalCount);
            Assert.Equal(0, fail_6.TotalCount);
        }
    }
}
