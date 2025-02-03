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

        public async Task<Guid?> CreateOrderElement(OrderElementRequest orderElement)
        {
            Entity.OrderElement? orderElementEntity = await _context.OrderElements.FirstOrDefaultAsync(x => x.OrderId == orderElement.OrderId && x.ItemId == orderElement.ItemId);
            
            if (orderElementEntity != null)
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

        public async Task<bool> UpdateOrderElement(Guid id, OrderElementRequest orderElement)
        {
            Entity.OrderElement? orderElementEntity = await _context.OrderElements.FirstOrDefaultAsync(x => x.Id == id);

            if (orderElementEntity is null) return false;

            orderElementEntity.UpdateInDb(orderElement);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteOrderElement(Guid id)
        {
            Entity.OrderElement? orderElementEntity = await _context.OrderElements.FirstOrDefaultAsync(x => x.Id == id);

            if (orderElementEntity is null) return false;

            _context.OrderElements.Remove(orderElementEntity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<OrderElementResponse>> GetOrderElementsByOrderId(Guid id)
        {
            List<OrderElementResponse> result = await _context.OrderElements.Where(x => x.OrderId == id)
                                                                            .Include(x => x.Item)
                                                                            .Select(x => x.MapFromDb())
                                                                            .ToListAsync();

            return result ?? [];
        }
    }
}
