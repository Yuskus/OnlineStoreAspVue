using OnlineStore.Server.Tests.Common;

namespace OnlineStore.Server.Tests.Repositories.Item
{
    [Collection("DatabaseCollection")]
    public class ItemRepositoryTest : IClassFixture<ItemDbContextFixture>
    {
        /*Task<ResponseList<ItemResponse>> GetPageOfItems(int pageNumber, int pageSize);
        Task<ItemResponse?> GetItemById(Guid id);
        Task<ItemResponse?> GetItemByCode(string code);
        Task<ItemResponse?> GetItemByName(string name);
        Task<ResponseList<ItemResponse>> GetPageOfItemsByCategory(string category, int pageNumber, int pageSize);
        Task<Guid?> CreateItem(ItemRequest item);
        Task<bool> UpdateItem(Guid id, ItemRequest item);
        Task<bool> DeleteItem(Guid id);
        ImmutableSortedSet<string> GetAllCategories();*/

        private readonly FakeDbContext _context;

        public ItemRepositoryTest(ItemDbContextFixture fixture)
        {
            _context = fixture.Context;
        }
    }
}
