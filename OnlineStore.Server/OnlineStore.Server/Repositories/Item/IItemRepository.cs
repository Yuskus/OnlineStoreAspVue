using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Item;
using System.Collections.Immutable;

namespace OnlineStore.Server.Repositories.Item
{
    public interface IItemRepository
    {
        Task<ResponseList<ItemResponse>> GetAllItems();
        Task<ResponseList<ItemResponse>> GetItemsByCriteria(ItemFilterCriteria criteria);
        Task<ItemResponse?> GetOneByCriteria(ItemFilterCriteria criteria);
        Task<Guid?> CreateItem(ItemRequest item);
        Task<bool> UpdateItem(Guid id, ItemRequest item);
        Task<bool> DeleteItem(Guid id);
        ImmutableSortedSet<string> GetAllCategories();
    }
}
