using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Database.Context;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Items;
using OnlineStore.Server.Mapping.Items;
using System.Collections.Immutable;
using OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Repositories.Items
{
    public class ItemRepository(OnlineStoreDbContext context) : IItemRepository
    {
        private readonly OnlineStoreDbContext _context = context;

        public async Task<Guid?> Create(ItemRequest item)
        {
            Item? itemEntity = await _context.Items.FirstOrDefaultAsync(x => x.Code == item.Code);

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
            if (await _context.Items.FirstOrDefaultAsync(x => x.Id == id) is Item itemEntity)
            {
                itemEntity.UpdateInDb(item);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            if (await _context.Items.FirstOrDefaultAsync(x => x.Id == id) is Item item)
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

        public async Task<ResponseList<ItemResponse>> GetPage(PageInfo pageInfo)
        {
            IQueryable<Item> query = _context.Items
                .AsSingleQuery();

            return new()
            {
                Responses = await query
                    .Skip((pageInfo.Number - 1) * pageInfo.Size)
                    .Take(pageInfo.Size)
                    .Select(x => x.MapFromDb())
                    .ToListAsync(),
                TotalCount = await query
                    .CountAsync()
            };
        }

        public async Task<ResponseList<ItemResponse>> GetPageByCriteria(ItemFilterCriteria criteria, PageInfo pageInfo)
        {
            IQueryable<Item> filtredQuery = FilteringItems(criteria);

            return new()
            {
                Responses = await filtredQuery
                    .Skip((pageInfo.Number - 1) * pageInfo.Size)
                    .Take(pageInfo.Size)
                    .Select(x => x.MapFromDb())
                    .ToListAsync(),
                TotalCount = await filtredQuery
                    .CountAsync()
            };
        }

        public async Task<ItemResponse?> GetOneByCriteria(ItemFilterCriteria criteria)
        {
            IQueryable<Item> filtredQuery = FilteringItems(criteria);

            return (await filtredQuery
                .FirstOrDefaultAsync())?.MapFromDb();
        }

        private IQueryable<Item> FilteringItems(ItemFilterCriteria criteria)
        {
            IQueryable<Item> items = _context.Items
                .AsSingleQuery();

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

            return items;
        }
    }
}
