using OnlineStore.Server.DTO.Items;
using OnlineStore.Server.Repositories.Items;
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
        public async Task GetOneByCriteria()
        {
            // Arrange
            var repository = new ItemRepository(_context);

            // Act
            var getById_Success = await repository.GetOneByCriteria(new() { Id = _fixture.ItemId_Exists });
            var getById_Fail = await repository.GetOneByCriteria(new() { Id = _fixture.ItemId_Unexists });

            var getByName_Success = await repository.GetOneByCriteria(new() { Name = _fixture.ItemName_Exists });
            var getByName_Fail = await repository.GetOneByCriteria(new() { Name = _fixture.ItemName_Unexists });

            var getByCode_Success = await repository.GetOneByCriteria(new() { Code = _fixture.ItemCode_Exists });
            var getByCode_Fail = await repository.GetOneByCriteria(new() { Code = _fixture.ItemCode_Unexists });

            // Assert
            Assert.NotNull(getById_Success);
            Assert.Null(getById_Fail);

            Assert.NotNull(getByName_Success);
            Assert.Null(getByName_Fail);

            Assert.NotNull(getByCode_Success);
            Assert.Null(getByCode_Fail);
        }

        [Fact]
        public async Task GetPageOfItems()
        {
            // Arrange
            var repository = new ItemRepository(_context);

            // Act
            var test = await repository.GetAll();

            // Assert
            // there may be range of values because of "create item" test
            Assert.InRange(test.TotalCount, _fixture.ItemsTotalCount - 1, _fixture.ItemsTotalCount + 1);
            Assert.InRange(test.Responses.Count(), _fixture.ItemsTotalCount - 1, _fixture.ItemsTotalCount + 1);
        }

        [Fact]
        public async Task GetAllByCriteria_Success()
        {
            // Arrange
            var repository = new ItemRepository(_context);

            var request_1 = new ItemFilterCriteria { Category = _fixture.Category_SampleA };
            var request_2 = new ItemFilterCriteria { Category = _fixture.Category_SampleB };
            var request_3 = new ItemFilterCriteria { Category = _fixture.Category_SampleA, Code = _fixture.ItemCode_Exists };
            var request_4 = new ItemFilterCriteria { Category = _fixture.Category_SampleB, Name = _fixture.ItemName_Exists };

            // Act
            var test_1 = await repository.GetAllByCriteria(request_1);
            var test_2 = await repository.GetAllByCriteria(request_2);
            var test_3 = await repository.GetAllByCriteria(request_3);
            var test_4 = await repository.GetAllByCriteria(request_4);

            // Assert
            Assert.NotNull(test_1);
            Assert.NotNull(test_2);
            Assert.NotNull(test_3);
            Assert.NotNull(test_4);

            Assert.InRange(test_1.TotalCount, 1, _fixture.ItemsTotalCount);
            Assert.InRange(test_2.TotalCount, 1, _fixture.ItemsTotalCount);
            Assert.InRange(test_3.TotalCount, 1, _fixture.ItemsTotalCount);
            Assert.InRange(test_4.TotalCount, 1, _fixture.ItemsTotalCount);

            Assert.InRange(test_1.Responses.Count(), 1, _fixture.ItemsTotalCount);
            Assert.InRange(test_2.Responses.Count(), 1, _fixture.ItemsTotalCount);
            Assert.InRange(test_3.Responses.Count(), 1, _fixture.ItemsTotalCount);
            Assert.InRange(test_4.Responses.Count(), 1, _fixture.ItemsTotalCount);
        }

        [Fact]
        public async Task GetAllByCriteria_Fail()
        {
            // Arrange
            var repository = new ItemRepository(_context);

            var request_1 = new ItemFilterCriteria { Category = _fixture.Category_Unexists };
            var request_2 = new ItemFilterCriteria { Category = _fixture.Category_SampleA, Name = _fixture.ItemName_Unexists };
            var request_3 = new ItemFilterCriteria { Category = _fixture.Category_SampleB, Code = _fixture.ItemCode_Unexists };

            // Act
            var test_1 = await repository.GetAllByCriteria(request_1);
            var test_2 = await repository.GetAllByCriteria(request_2);
            var test_3 = await repository.GetAllByCriteria(request_3);

            // Assert
            Assert.NotNull(test_1);
            Assert.NotNull(test_2);
            Assert.NotNull(test_3);

            Assert.Equal(0, test_1.TotalCount);
            Assert.Equal(0, test_2.TotalCount);
            Assert.Equal(0, test_3.TotalCount);

            Assert.Empty(test_1.Responses);
            Assert.Empty(test_2.Responses);
            Assert.Empty(test_3.Responses);
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
            var createNew = await repository.Create(request);
            var createSame = await repository.Create(request);

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
            var updateItem_Fail = await repository.Update(_fixture.ItemId_Unexists, requestNew);
            var updateItem_Success = await repository.Update(_fixture.ItemId_ForUpdate, requestNew);

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
            var deleteItem_Fail_1 = await repository.Delete(_fixture.ItemId_Unexists);
            var deleteItem_Success = await repository.Delete(_fixture.ItemId_ForDelete);
            var deleteItem_Fail_2 = await repository.Delete(_fixture.ItemId_ForDelete);

            // Assert
            Assert.False(deleteItem_Fail_1);
            Assert.True(deleteItem_Success);
            Assert.False(deleteItem_Fail_2);
        }
    }
}
