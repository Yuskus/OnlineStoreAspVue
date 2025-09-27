using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Item;
using System.Collections.Immutable;

namespace OnlineStore.Server.Services.Item
{
    public interface IItemService
    {
        Task<Guid?> Create(ItemRequest request);
        Task<bool> Update(Guid id, ItemRequest request);
        Task<bool> Delete(Guid id);
        Task<ResponseList<ItemResponse>> GetPage(int page, int pageSize);
        Task<ResponseList<ItemResponse>> GetPageByCriteria(ItemFilterCriteria criteria, int page, int pageSize);
        Task<ItemResponse?> GetOneByCriteria(ItemFilterCriteria criteria);
        ImmutableSortedSet<string> GetAllCategories();
    }
}
