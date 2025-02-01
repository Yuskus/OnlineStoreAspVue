using OnlineStore.Server.DTO.Item;
using System.Collections.Immutable;

namespace OnlineStore.Server.Services.Item
{
    public interface IItemService
    {
        Task<ItemResponseList> GetPageOfItems(int pageNumber, int pageSize);
        Task<ItemResponse?> GetItemById(Guid id);
        Task<ItemResponse?> GetItemByCode(string code);
        Task<ItemResponse?> GetItemByName(string name);
        Task<ItemResponseList> GetPageOfItemsByCategory(string category, int pageNumber, int pageSize);
        Task<Guid?> CreateItem(ItemRequest item);
        Task<bool> UpdateItem(Guid id, ItemRequest item);
        Task<bool> DeleteItem(Guid id);
        ImmutableSortedSet<string> GetAllCategories();
    }
}
