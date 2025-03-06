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
            var getById_Success = await repository.GetOneByCriteria(new() { Id = _fixture.ItemId_Exists });
            var getById_Fail = await repository.GetOneByCriteria(new() { Id = _fixture.ItemId_Unexists });

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
            var getByName_Success = await repository.GetOneByCriteria(new() { Name = _fixture.ItemName_Exists });
            var getByName_Fail = await repository.GetOneByCriteria(new() { Name = _fixture.ItemName_Unexists });

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
            var getByCode_Success = await repository.GetOneByCriteria(new() { Code = _fixture.ItemCode_Exists });
            var getByCode_Fail = await repository.GetOneByCriteria(new() { Code = _fixture.ItemCode_Unexists });

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
            var test = await repository.GetAllItems();

            // Assert
            // there may be range of values because of "create item" test
            Assert.InRange(test.TotalCount, _fixture.ItemsTotalCount - 1, _fixture.ItemsTotalCount + 1);
            Assert.InRange(test.Responses.Count(), _fixture.ItemsTotalCount - 1, _fixture.ItemsTotalCount + 1);
        }

        [Fact]
        public async Task GetPageOfItemsByCategory()
        {
            // Arrange
            var repository = new ItemRepository(_context);

            // Act
            var unexist = await repository.GetItemsByCriteria(new() { Category = _fixture.Category_Unexists });

            var categoryA = await repository.GetItemsByCriteria(new() { Category = _fixture.Category_SampleA });
            var categoryB = await repository.GetItemsByCriteria(new() { Category = _fixture.Category_SampleB });

            // Assert
            Assert.NotNull(unexist);
            Assert.NotNull(categoryA);
            Assert.NotNull(categoryB);

            Assert.Equal(0, unexist.TotalCount);
            Assert.InRange(categoryA.TotalCount, 1, _fixture.ItemsTotalCount);
            Assert.InRange(categoryB.TotalCount, 1, _fixture.ItemsTotalCount);

            Assert.Empty(unexist.Responses);
            Assert.InRange(categoryA.Responses.Count(), 1, _fixture.ItemsTotalCount);
            Assert.InRange(categoryB.Responses.Count(), 1, _fixture.ItemsTotalCount);
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
