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

        public async Task<Guid?> Create(ItemRequest item)
        {
            bool isValid = ItemValidator.CheckRequest(item);

            if (isValid)
            {
                return await _itemRepository.Create(item);
            }
            
            return null;
        }

        public async Task<bool> Update(Guid id, ItemRequest item)
        {
            bool isValid = ItemValidator.CheckGuid(id)
                        && ItemValidator.CheckRequest(item);

            if (isValid)
            {
                return await _itemRepository.Update(id, item);
            }

            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            bool isValid = ItemValidator.CheckGuid(id);

            if (isValid)
            {
                return await _itemRepository.Delete(id);
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

        public async Task<ResponseList<ItemResponse>> GetPageByCriteria(ItemFilterCriteria criteria, int pageNumber, int pageSize)
        {
            bool isValid = ItemValidator.CheckCriteria(criteria)
                        && ItemValidator.CheckPages(pageNumber, pageSize);

            if (isValid)
            {
                ResponseList<ItemResponse> response = await _itemRepository.GetAllByCriteria(criteria);

                response.Responses = response.Responses.Skip((pageNumber - 1) * pageSize)
                                                       .Take(pageSize)
                                                       .ToList();

                return response;
            }

            return new ResponseList<ItemResponse>();
        }

        public async Task<ResponseList<ItemResponse>> GetPage(int pageNumber, int pageSize)
        {
            bool isValid = ItemValidator.CheckPages(pageNumber, pageSize);

            if (isValid)
            {
                ResponseList<ItemResponse> response = await _itemRepository.GetAll();

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
