using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Item;
using OnlineStore.Server.Repositories.Item;
using OnlineStore.Server.Validation.Item;
using System.Collections.Immutable;

namespace OnlineStore.Server.Services.Item
{
    public class ItemService(IItemRepository itemRepository) : IItemService
    {
        private readonly IItemRepository _itemRepository = itemRepository;

        public async Task<Guid?> CreateItem(ItemRequest item)
        {
            bool isValid = ItemValidator.CheckRequest(item);

            if (isValid)
            {
                return await _itemRepository.CreateItem(item);
            }
            
            return null;
        }

        public async Task<bool> UpdateItem(Guid id, ItemRequest item)
        {
            bool isValid = ItemValidator.CheckGuid(id)
                        && ItemValidator.CheckRequest(item);

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

        public async Task<ItemResponse?> GetOneByCriteria(ItemFilterCriteria criteria)
        {
            bool isValid = ItemValidator.CheckCriteria(criteria);

            if (isValid)
            {
                return await _itemRepository.GetOneByCriteria(criteria);
            }

            return null;
        }

        public async Task<ResponseList<ItemResponse>> GetPageOfItemsByCriteria(ItemFilterCriteria criteria, int pageNumber, int pageSize)
        {
            bool isValid = ItemValidator.CheckCriteria(criteria)
                        && ItemValidator.CheckPages(pageNumber, pageSize);

            if (isValid)
            {
                ResponseList<ItemResponse> response = await _itemRepository.GetItemsByCriteria(criteria);

                response.Responses = response.Responses.Skip((pageNumber - 1) * pageSize)
                                                       .Take(pageSize)
                                                       .ToList();

                return response;
            }

            return new ResponseList<ItemResponse>();
        }

        public async Task<ResponseList<ItemResponse>> GetPageOfItems(int pageNumber, int pageSize)
        {
            bool isValid = ItemValidator.CheckPages(pageNumber, pageSize);

            if (isValid)
            {
                ResponseList<ItemResponse> response = await _itemRepository.GetAllItems();

                response.Responses = response.Responses.Skip((pageNumber - 1) * pageSize)
                                                       .Take(pageSize)
                                                       .ToList();

                return response;
            }

            return new ResponseList<ItemResponse>();
        }

        public ImmutableSortedSet<string> GetAllCategories()
        {
            return _itemRepository.GetAllCategories();
        }
    }
}
