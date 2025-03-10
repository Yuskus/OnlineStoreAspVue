namespace OnlineStore.Server.Tests.Services.Item
{
    [Collection("ItemServiceCollection")]
    public class ItemServiceTest : IClassFixture<ItemServiceFixture>
    {
        /*Task<Guid?> Create(ItemRequest item);
        Task<bool> Update(Guid id, ItemRequest item);
        Task<bool> Delete(Guid id);
        Task<ResponseList<ItemResponse>> GetPage(int pageNumber, int pageSize);
        Task<ResponseList<ItemResponse>> GetPageByCriteria(ItemFilterCriteria criteria, int pageNumber, int pageSize);
        Task<ItemResponse?> GetOneByCriteria(ItemFilterCriteria criteria);
        ImmutableSortedSet<string> GetAllCategories();*/

        private readonly ItemServiceFixture _fixture;
        public ItemServiceTest(ItemServiceFixture fixture)
        {
            _fixture = fixture;
        }
    }
}
