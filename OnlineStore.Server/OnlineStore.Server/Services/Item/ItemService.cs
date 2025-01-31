using OnlineStore.Server.DTO.Item;
using OnlineStore.Server.Repositories.Item;
using OnlineStore.Server.Validation.Item;

namespace OnlineStore.Server.Services.Item
{
    public class ItemService(IItemRepository itemRepository) : IItemService
    {
        private readonly IItemRepository _itemRepository = itemRepository;

        public async Task<Guid?> CreateItem(ItemRequest item)
        {
            bool isValid = ItemValidator.CheckCode(item.Code)
                        && ItemValidator.CheckPrice(item.Price);

            if (isValid)
            {
                return await _itemRepository.CreateItem(item);
            }
            
            return null;
        }

        public async Task<bool> UpdateItem(Guid id, ItemRequest item)
        {
            bool isValid = ItemValidator.CheckGuid(id)
                        && ItemValidator.CheckCode(item.Code)
                        && ItemValidator.CheckPrice(item.Price);

            if (isValid)
            {
                return await _itemRepository.UpdateItem(id, item);
            }

            return false;
        }

        public async Task<bool> DeleteItem(Guid id)
        {
            bool isValid = ItemValidator.CheckGuid(id);

            if (isValid)
            {
                return await _itemRepository.DeleteItem(id);
            }

            return false;
        }

        public async Task<ItemResponse?> GetItemByCode(string code)
        {
            bool isValid = ItemValidator.CheckCode(code);

            if (isValid)
            {
                return await _itemRepository.GetItemByCode(code);
            }

            return null;
        }

        public async Task<ItemResponse?> GetItemById(Guid id)
        {
            bool isValid = ItemValidator.CheckGuid(id);

            if (isValid)
            {
                return await _itemRepository.GetItemById(id);
            }

            return null;
        }

        public async Task<ItemResponse?> GetItemByName(string name)
        {
            return await _itemRepository.GetItemByName(name);
        }

        public async Task<IEnumerable<ItemResponse>> GetPageOfItemsByCategory(string category, int pageNumber, int pageSize)
        {
            bool isValid = ItemValidator.CheckPages(pageNumber, pageSize);

            if (isValid)
            {
                return await _itemRepository.GetPageOfItemsByCategory(category, pageNumber, pageSize);
            }

            return [];
        }

        public async Task<IEnumerable<ItemResponse>> GetPageOfItems(int pageNumber, int pageSize)
        {
            bool isValid = ItemValidator.CheckPages(pageNumber, pageSize);

            if (isValid)
            {
                return await _itemRepository.GetPageOfItems(pageNumber, pageSize);
            }

            return [];
        }
    }
}
