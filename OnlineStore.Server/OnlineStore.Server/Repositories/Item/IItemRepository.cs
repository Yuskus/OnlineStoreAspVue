using OnlineStore.Server.DTO.Item;

namespace OnlineStore.Server.Repositories.Item
{
    public interface IItemRepository
    {
        Task<IEnumerable<ItemResponse>> GetPageOfItems(int pageNumber, int pageSize); //add count() to response
        Task<ItemResponse?> GetItemById(Guid id);
        Task<ItemResponse?> GetItemByCode(string code);
        Task<ItemResponse?> GetItemByName(string name);
        Task<IEnumerable<ItemResponse>> GetPageOfItemsByCategory(string category, int pageNumber, int pageSize); //add count() to response
        Task<Guid?> CreateItem(ItemRequest item);
        Task<bool> UpdateItem(Guid id, ItemRequest item);
        Task<bool> DeleteItem(Guid id);
    }
}
