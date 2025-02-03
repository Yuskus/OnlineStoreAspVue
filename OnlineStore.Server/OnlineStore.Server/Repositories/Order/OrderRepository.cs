using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Database.Context;
using OnlineStore.Server.DTO.Order;
using OnlineStore.Server.Mapping.Order;
using System.Text.Json;
using Entity = OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Repositories.Order
{
    public class OrderRepository(OnlineStoreDbContext context) : IOrderRepository
    {
        private readonly OnlineStoreDbContext _context = context;

        public async Task<Guid?> CreateOrder(OrderRequest order)
        {
            int total = _context.Orders.Count() + 1;

            Entity.Order orderEntity = order.MapToDb(total);

            await _context.Orders.AddAsync(orderEntity);
            await _context.SaveChangesAsync();

            return orderEntity.Id;
        }

        public async Task<bool> UpdateOrder(Guid id, OrderRequest order)
        {
            Entity.Order? orderEntity = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);

            if (orderEntity is null) return false;

            orderEntity.UpdateInDb(order);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> PlaceAnOrder(Guid orderId)
        {
            Entity.Order? orderEntity = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);

            if (orderEntity is null) return false;

            orderEntity.OrderStatus = "in progress";
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteOrder(Guid id)
        {
            Entity.Order? orderEntity = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);

            if (orderEntity is null) return false;

            _context.Orders.Remove(orderEntity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<OrderResponse?> GetOrderByNumber(int number)
        {
            Entity.Order? orderEntity = await _context.Orders.Include(x => x.Customer)
                                                             .FirstOrDefaultAsync(x => x.OrderNumber == number);

            if (orderEntity is null) return null;

            OrderResponse result = orderEntity.MapFromDb();
            result.CustomerName = orderEntity.Customer?.Name;

            return result;
        }

        public async Task<OrderResponseList> GetPageOfOrders(int pageNumber, int pageSize)
        {
            List<OrderResponse> response = await _context.Orders.Skip((pageNumber - 1) * pageSize)
                                                                .Take(pageSize)
                                                                .Select(x => x.MapFromDb()).ToListAsync();

            int totalCount = await _context.Orders.CountAsync();

            OrderResponseList result = new(response, totalCount);

            return result;
        }

        public async Task<OrderResponseList> GetPageOfOrdersByCustomerId(Guid id, int pageNumber, int pageSize)
        {
            List<OrderResponse> response = await _context.Orders.Where(x => x.CustomerId == id)
                                                                .Skip((pageNumber - 1) * pageSize)
                                                                .Take(pageSize)
                                                                .Select(x => x.MapFromDb()).ToListAsync();

            int totalCount = await _context.Orders.CountAsync(x => x.CustomerId == id);

            OrderResponseList result = new(response, totalCount);

            return result;
        }

        public async Task<OrderResponseList> GetPageOfOrdersByStatus(string status, int pageNumber, int pageSize)
        {
            List<OrderResponse> response = await _context.Orders.Where(x => x.OrderStatus == status)
                                                                .Skip((pageNumber - 1) * pageSize)
                                                                .Take(pageSize)
                                                                .Select(x => x.MapFromDb()).ToListAsync();

            int totalCount = await _context.Orders.CountAsync(x => x.OrderStatus == status);

            OrderResponseList result = new(response, totalCount);

            return result;
        }

        public async Task<OrderResponse?> GetBasketOrder(Guid customerId)
        {
            // поиск корзины (заказ со статусом new)
            Entity.Order? order = await _context.Orders.Where(x => x.CustomerId == customerId && x.OrderStatus != null && x.OrderStatus.ToLower() == "new")
                                                       .SingleOrDefaultAsync();

            if (order is null)
            {
                // создание при отсутствии
                order = new Entity.Order(customerId, _context.Orders.Count() + 1);
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
            }

            OrderResponse orderResponse = order.MapFromDb();
            orderResponse.CustomerName = order.Customer?.Name;

            return orderResponse;
        }
    }
}
