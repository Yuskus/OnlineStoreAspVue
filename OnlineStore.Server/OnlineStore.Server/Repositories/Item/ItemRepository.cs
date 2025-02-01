using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Database.Context;
using OnlineStore.Server.DTO.Item;
using OnlineStore.Server.Mapping.Item;
using Entity = OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Repositories.Item
{
    public class ItemRepository(OnlineStoreDbContext context) : IItemRepository
    {
        private readonly OnlineStoreDbContext _context = context;

        public async Task<Guid?> CreateItem(ItemRequest item)
        {
            Entity.Item? itemEntity = await _context.Items.FirstOrDefaultAsync(x => x.Code == item.Code);
            if (itemEntity is null)
            {
                itemEntity = item.MapToDb();

                await _context.Items.AddAsync(itemEntity);
                await _context.SaveChangesAsync();
            }

            return itemEntity.Id;
        }

        public async Task<bool> UpdateItem(Guid id, ItemRequest item)
        {
            Entity.Item? itemEntity = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);

            if (itemEntity is null) return false;

            itemEntity.UpdateInDb(item);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteItem(Guid id)
        {
            Entity.Item? itemEntity = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);

            if (itemEntity is null) return false;

            _context.Items.Remove(itemEntity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ItemResponse?> GetItemByCode(string code)
        {
            Entity.Item? itemEntity = await _context.Items.FirstOrDefaultAsync(x => x.Code == code);

            if (itemEntity is null) return null;

            ItemResponse result = itemEntity.MapFromDb();

            return result;
        }

        public async Task<ItemResponse?> GetItemById(Guid id)
        {
            Entity.Item? itemEntity = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);

            if (itemEntity is null) return null;

            ItemResponse result = itemEntity.MapFromDb();

            return result;
        }

        public async Task<ItemResponse?> GetItemByName(string name)
        {
            Entity.Item? itemEntity = await _context.Items.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());

            if (itemEntity is null) return null;

            ItemResponse result = itemEntity.MapFromDb();

            return result;
        }

        public async Task<ItemResponseList> GetPageOfItemsByCategory(string category, int pageNumber, int pageSize)
        {
            List<ItemResponse> response = await _context.Items.Where(x => x.Category != null && x.Category.ToLower() == category.ToLower())
                                                            .Skip((pageNumber - 1) * pageSize)
                                                            .Take(pageSize)
                                                            .Select(x => x.MapFromDb()).ToListAsync();

            int totalCount = await _context.Items.CountAsync(x => x.Category != null && x.Category.ToLower() == category.ToLower());

            ItemResponseList result = new(response, totalCount);

            return result;
        }

        public async Task<ItemResponseList> GetPageOfItems(int pageNumber, int pageSize)
        {
            List<ItemResponse> response = await _context.Items.Skip((pageNumber - 1) * pageSize)
                                                            .Take(pageSize)
                                                            .Select(x => x.MapFromDb()).ToListAsync();

            int totalCount = await _context.Items.CountAsync();

            ItemResponseList result = new(response, totalCount);

            return result;
        }
    }
}
