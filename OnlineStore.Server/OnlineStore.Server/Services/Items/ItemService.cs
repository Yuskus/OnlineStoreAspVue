using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Items;
using OnlineStore.Server.Repositories.Items;
using OnlineStore.Server.Validation.Items;
using System.Collections.Immutable;

namespace OnlineStore.Server.Services.Items
{
    public class ItemService(IItemRepository itemRepository) : IItemService
    {
        private readonly IItemRepository _itemRepository = itemRepository;

        public async Task<Guid?> Create(ItemRequest request)
        {
            bool isValid = ItemValidator.CheckRequest(request);

            if (isValid)
            {
                return await _itemRepository.Create(request);
            }
            
            return null;
        }

        public async Task<bool> Update(Guid id, ItemRequest request)
        {
            bool isValid = ItemValidator.CheckGuid(id)
                        && ItemValidator.CheckRequest(request);

            if (isValid)
            {
                return await _itemRepository.Update(id, request);
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

        public async Task<ResponseList<ItemResponse>> GetPageByCriteria(ItemFilterCriteria criteria, int page, int pageSize)
        {
            bool isValid = ItemValidator.CheckCriteria(criteria)
                        && ItemValidator.CheckPages(page, pageSize);

            if (isValid)
            {
                ResponseList<ItemResponse> response = await _itemRepository.GetAllByCriteria(criteria);

                response.Responses = [.. response.Responses.Skip((page - 1) * pageSize).Take(pageSize)];

                return response;
            }

            return new ResponseList<ItemResponse>();
        }

        public async Task<ResponseList<ItemResponse>> GetPage(int page, int pageSize)
        {
            bool isValid = ItemValidator.CheckPages(page, pageSize);

            if (isValid)
            {
                ResponseList<ItemResponse> response = await _itemRepository.GetAll();

                response.Responses = [.. response.Responses.Skip((page - 1) * pageSize).Take(pageSize)];

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
