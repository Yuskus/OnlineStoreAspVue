using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Database.Context;
using OnlineStore.Server.DTO.OrderElement;
using OnlineStore.Server.Mapping.Order;
using OnlineStore.Server.Mapping.OrderElement;
using Entity = OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Repositories.OrderElement
{
    public class OrderElementRepository(OnlineStoreDbContext context) : IOrderElementRepository
    {
        private readonly OnlineStoreDbContext _context = context;

        public async Task<Guid?> Create(OrderElementRequest orderElement)
        {
            if (await _context.Orders.AnyAsync(x => x.Id == orderElement.OrderId) == false) return null;
            if (await _context.Items.AnyAsync(x => x.Id == orderElement.ItemId) == false) return null;

            Entity.OrderElement? orderElementEntity = await _context.OrderElements.FirstOrDefaultAsync(x => x.OrderId == orderElement.OrderId 
                                                                                                         && x.ItemId == orderElement.ItemId);
            
            if (orderElementEntity is not null)
            {
                orderElementEntity.ItemsCount++;
            }
            else
            {
                orderElementEntity = orderElement.MapToDb();
                await _context.OrderElements.AddAsync(orderElementEntity);
            }

            await _context.SaveChangesAsync();

            return orderElementEntity.Id;
        }

        public async Task<bool> Update(Guid id, UpdateOrderElementRequest orderElement)
        {
            if (await _context.OrderElements.FirstOrDefaultAsync(x => x.Id == id) is Entity.OrderElement orderElementEntity)
            {
                orderElementEntity.UpdateInDb(orderElement);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            Entity.OrderElement? orderElementEntity = await _context.OrderElements.FirstOrDefaultAsync(x => x.Id == id);

            if (orderElementEntity is null) return false;

            _context.OrderElements.Remove(orderElementEntity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<OrderElementResponse>> GetAllByOrderId(Guid id)
        {
            return await _context.OrderElements.Where(x => x.OrderId == id)
                                               .Include(x => x.Item)
                                               .Select(x => x.MapFromDb())
                                               .ToListAsync();
        }
    }
}
