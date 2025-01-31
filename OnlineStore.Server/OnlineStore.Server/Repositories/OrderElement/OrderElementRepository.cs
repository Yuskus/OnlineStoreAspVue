using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Database.Context;
using OnlineStore.Server.DTO.Order;
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
            Entity.OrderElement orderElementEntity = orderElement.MapToDb();

            await _context.OrderElements.AddAsync(orderElementEntity);
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

        public async Task<IEnumerable<OrderElementResponse>> GetBasketByCustomerId(Guid id)
        {
            Entity.Customer? customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);

            Entity.Order? order = customer?.Orders.LastOrDefault();

            IEnumerable<OrderElementResponse>? result = order?.OrderElements.Select(x => x.MapFromDb());

            return result ?? [];
        }

        public async Task<IEnumerable<OrderElementResponse>> GetOrderElementsByOrderId(Guid id)
        {
            Entity.Order? order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);

            List<OrderElementResponse>? res = order?.OrderElements.Select(x => x.MapFromDb()).ToList();

            return res ?? [];
        }
    }
}
