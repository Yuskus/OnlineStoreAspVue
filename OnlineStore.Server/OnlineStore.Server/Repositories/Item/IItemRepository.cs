using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Item;
using System.Collections.Immutable;

namespace OnlineStore.Server.Repositories.Item
{
    public interface IItemRepository
    {
        Task<Guid?> Create(ItemRequest item);
        Task<bool> Update(Guid id, ItemRequest item);
        Task<bool> Delete(Guid id);
        Task<ResponseList<ItemResponse>> GetAll();
        Task<ResponseList<ItemResponse>> GetAllByCriteria(ItemFilterCriteria criteria);
        Task<ItemResponse?> GetOneByCriteria(ItemFilterCriteria criteria);
        ImmutableSortedSet<string> GetAllCategories();
    }
}
