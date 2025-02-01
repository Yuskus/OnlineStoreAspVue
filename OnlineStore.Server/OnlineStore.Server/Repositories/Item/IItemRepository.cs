using OnlineStore.Server.DTO.Item;

namespace OnlineStore.Server.Repositories.Item
{
    public interface IItemRepository
    {
        Task<ItemResponseList> GetPageOfItems(int pageNumber, int pageSize);
        Task<ItemResponse?> GetItemById(Guid id);
        Task<ItemResponse?> GetItemByCode(string code);
        Task<ItemResponse?> GetItemByName(string name);
        Task<ItemResponseList> GetPageOfItemsByCategory(string category, int pageNumber, int pageSize);
        Task<Guid?> CreateItem(ItemRequest item);
        Task<bool> UpdateItem(Guid id, ItemRequest item);
        Task<bool> DeleteItem(Guid id);
    }
}
