using OnlineStore.Server.DTO.Item;
using OnlineStore.Server.Repositories.Item;
using OnlineStore.Server.Tests.Common;

namespace OnlineStore.Server.Tests.Repositories.Item
{
    [Collection("DatabaseCollection")]
    public class ItemRepositoryTest : IClassFixture<ItemDbContextFixture>
    {
        private readonly FakeDbContext _context;
        private readonly ItemDbContextFixture _fixture;

        public ItemRepositoryTest(ItemDbContextFixture fixture)
        {
            _fixture = fixture;
            _context = _fixture.Context;
        }

        [Fact]
        public async Task GetItemById()
        {
            // Arrange
            var repository = new ItemRepository(_context);

            // Act
            var getById_Success = await repository.GetItemById(_fixture.ItemId_Exists);
            var getById_Fail = await repository.GetItemById(_fixture.ItemId_Unexists);

            // Assert
            Assert.NotNull(getById_Success);
            Assert.Null(getById_Fail);
        }

        [Fact]
        public async Task GetItemByName()
        {
            // Arrange
            var repository = new ItemRepository(_context);

            // Act
            var getByName_Success = await repository.GetItemByName(_fixture.ItemName_Exists);
            var getByName_Fail = await repository.GetItemByName(_fixture.ItemName_Unexists);

            // Assert
            Assert.NotNull(getByName_Success);
            Assert.Null(getByName_Fail);
        }

        [Fact]
        public async Task GetItemByCode()
        {
            // Arrange
            var repository = new ItemRepository(_context);

            // Act
            var getByCode_Success = await repository.GetItemByCode(_fixture.ItemCode_Exists);
            var getByCode_Fail = await repository.GetItemByCode(_fixture.ItemCode_Unexists);

            // Assert
            Assert.NotNull(getByCode_Success);
            Assert.Null(getByCode_Fail);
        }

        [Fact]
        public async Task GetPageOfItems()
        {
            // Arrange
            var repository = new ItemRepository(_context);

            // Act
            var test1 = await repository.GetPageOfItems(1, 5);
            var test2 = await repository.GetPageOfItems(2, 5);
            var test3 = await repository.GetPageOfItems(3, 3);
            var test4 = await repository.GetPageOfItems(1, 20);
            var test5 = await repository.GetPageOfItems(5, 1);

            // Assert
            // there may be range of values because of "create item" test
            Assert.InRange(test1.TotalCount, _fixture.ItemsTotalCount - 1, _fixture.ItemsTotalCount + 1);
            Assert.InRange(test2.TotalCount, _fixture.ItemsTotalCount - 1, _fixture.ItemsTotalCount + 1);
            Assert.InRange(test3.TotalCount, _fixture.ItemsTotalCount - 1, _fixture.ItemsTotalCount + 1);
            Assert.InRange(test4.TotalCount, _fixture.ItemsTotalCount - 1, _fixture.ItemsTotalCount + 1);
            Assert.InRange(test5.TotalCount, _fixture.ItemsTotalCount - 1, _fixture.ItemsTotalCount + 1);

            Assert.Equal(5, test1.Responses.Count());
            Assert.Equal(5, test2.Responses.Count());
            Assert.Equal(3, test3.Responses.Count());
            Assert.Equal(20, test4.Responses.Count());
            Assert.Single(test5.Responses);
        }

        [Fact]
        public async Task GetPageOfItemsByCategory()
        {
            // Arrange
            var repository = new ItemRepository(_context);

            // Act
            var unexist = await repository.GetPageOfItemsByCategory(_fixture.Category_Unexists, 1, 5);

            var categoryA = await repository.GetPageOfItemsByCategory(_fixture.Category_SampleA, 1, 5);
            var categoryB = await repository.GetPageOfItemsByCategory(_fixture.Category_SampleB, 1, 7);

            // Assert
            Assert.NotNull(unexist);
            Assert.NotNull(categoryA);
            Assert.NotNull(categoryB);

            Assert.Equal(0, unexist.TotalCount);
            Assert.InRange(categoryA.TotalCount, 1, _fixture.ItemsTotalCount);
            Assert.InRange(categoryB.TotalCount, 1, _fixture.ItemsTotalCount);

            Assert.Empty(unexist.Responses);
            Assert.InRange(categoryA.Responses.Count(), 1, 5);
            Assert.InRange(categoryB.Responses.Count(), 1, 7);
        }

        [Fact]
        public async Task CreateItem()
        {
            // Arrange
            var repository = new ItemRepository(_context);

            var request = new ItemRequest
            {
                Name = "Bananas",
                Category = "Fruits",
                Code = "77-7777-BA77",
                Price = 50
            };

            // Act
            var createNew = await repository.CreateItem(request);
            var createSame = await repository.CreateItem(request);

            // Assert
            Assert.NotNull(createNew);
            Assert.NotNull(createSame);

            Assert.NotEqual(createNew, Guid.Empty);
            Assert.NotEqual(createSame, Guid.Empty);

            Assert.Equal(createNew, createSame);
        }

        [Fact]
        public async Task UpdateItem()
        {
            // Arrange
            var repository = new ItemRepository(_context);

            var requestNew = new ItemRequest
            {
                Name = "Apple",
                Category = "Fruits",
                Code = "88-5555-YU00",
                Price = 40
            };

            // Act
            var updateItem_Fail = await repository.UpdateItem(_fixture.ItemId_Unexists, requestNew);
            var updateItem_Success = await repository.UpdateItem(_fixture.ItemId_ForUpdate, requestNew);

            // Assert
            Assert.False(updateItem_Fail);
            Assert.True(updateItem_Success);
        }

        [Fact]
        public async Task DeleteItem()
        {
            // Arrange
            var repository = new ItemRepository(_context);

            // Act
            var deleteItem_Fail = await repository.DeleteItem(_fixture.ItemId_Unexists);
            var deleteItem_Success = await repository.DeleteItem(_fixture.ItemId_ForDelete);
            var deleteItem_Again = await repository.DeleteItem(_fixture.ItemId_ForDelete);

            // Assert
            Assert.False(deleteItem_Fail);
            Assert.True(deleteItem_Success);
            Assert.False(deleteItem_Again);
        }
    }
}
