using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Items;
using System.Collections.Immutable;

namespace OnlineStore.Server.Repositories.Items
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
