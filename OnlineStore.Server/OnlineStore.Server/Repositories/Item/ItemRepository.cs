using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Database.Context;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Item;
using OnlineStore.Server.Mapping.Item;
using System.Collections.Immutable;
using Entity = OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Repositories.Item
{
    public class ItemRepository(OnlineStoreDbContext context) : IItemRepository
    {
        private readonly OnlineStoreDbContext _context = context;

        public async Task<Guid?> Create(ItemRequest item)
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

        public async Task<bool> Update(Guid id, ItemRequest item)
        {
            if (await _context.Items.FirstOrDefaultAsync(x => x.Id == id) is Entity.Item itemEntity)
            {
                itemEntity.UpdateInDb(item);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            if (await _context.Items.FirstOrDefaultAsync(x => x.Id == id) is Entity.Item item)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public ImmutableSortedSet<string> GetAllCategories()
        {
            return [.. _context.Items.Select(x => x.Category ?? "") ];
        }

        public async Task<ResponseList<ItemResponse>> GetAll()
        {
            return new()
            {
                Responses = await _context.Items.Select(x => x.MapFromDb()).ToListAsync(),
                TotalCount = await _context.Items.CountAsync()
            };
        }

        public async Task<ResponseList<ItemResponse>> GetAllByCriteria(ItemFilterCriteria criteria)
        {
            IEnumerable<ItemResponse> filtred = await FilteringItems(criteria);

            return new()
            {
                Responses = filtred,
                TotalCount = filtred.Count()
            };
        }

        public async Task<ItemResponse?> GetOneByCriteria(ItemFilterCriteria criteria)
        {
            IEnumerable<ItemResponse> filtred = await FilteringItems(criteria);

            return filtred.FirstOrDefault();
        }

        private async Task<IEnumerable<ItemResponse>> FilteringItems(ItemFilterCriteria criteria)
        {
            IQueryable<Entity.Item> items = _context.Items;

            if (criteria.Id is not null)
            {
                items = items.Where(x => x.Id == criteria.Id);
            }
            if (criteria.Code is not null)
            {
                items = items.Where(x => x.Code == criteria.Code);
            }
            if (criteria.Category is not null)
            {
                items = items.Where(x => x.Category == criteria.Category);
            }
            if (criteria.Name is not null)
            {
                items = items.Where(x => x.Name.ToLower().Contains(criteria.Name.ToLower()));
            }

            return await items.Select(x => x.MapFromDb()).ToListAsync();
        }
    }
}
