using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Item;
using System.Collections.Immutable;

namespace OnlineStore.Server.Services.Item
{
    public interface IItemService
    {
        Task<ResponseList<ItemResponse>> GetPageOfItems(int pageNumber, int pageSize);
        Task<ResponseList<ItemResponse>> GetPageOfItemsByCriteria(ItemFilterCriteria criteria, int pageNumber, int pageSize);
        Task<ItemResponse?> GetOneByCriteria(ItemFilterCriteria criteria);
        Task<Guid?> CreateItem(ItemRequest item);
        Task<bool> UpdateItem(Guid id, ItemRequest item);
        Task<bool> DeleteItem(Guid id);
        ImmutableSortedSet<string> GetAllCategories();
    }
}
