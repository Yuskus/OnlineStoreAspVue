using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Items;
using System.Collections.Immutable;

namespace OnlineStore.Server.Services.Items
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
