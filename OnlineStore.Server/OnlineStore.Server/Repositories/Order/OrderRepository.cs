using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Database.Context;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Order;
using OnlineStore.Server.Mapping.Order;
using OnlineStore.Server.Utilities.Order.Generators;
using Entity = OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Repositories.Order
{
    public class OrderRepository(OnlineStoreDbContext context, INumberGenerator orderNumberGenerator) : IOrderRepository
    {
        private readonly OnlineStoreDbContext _context = context;
        private readonly INumberGenerator _orderNumberGenerator = orderNumberGenerator;

        public async Task<Guid?> CreateOrder(OrderRequest order)
        {
            int total = _orderNumberGenerator.GenerateNewNumber;

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
            Entity.Order? orderEntity = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId && x.OrderStatus == "basket");

            if (orderEntity is null) return false;

            orderEntity.OrderStatus = "new";
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
            Entity.Order? orderEntity = await _context.Orders.FirstOrDefaultAsync(x => x.OrderNumber == number);

            if (orderEntity is null) return null;

            OrderResponse result = orderEntity.MapFromDb();
            result.CustomerName = orderEntity.Customer?.Name;

            return result;
        }

        public async Task<ResponseList<OrderResponse>> GetPageOfOrders(int pageNumber, int pageSize)
        {
            List<OrderResponse> response = await _context.Orders.Include(x => x.Customer)
                                                                .Skip((pageNumber - 1) * pageSize)
                                                                .Take(pageSize)
                                                                .Select(x => x.MapFromDb()).ToListAsync();

            int totalCount = await _context.Orders.CountAsync();

            ResponseList<OrderResponse> result = new(response, totalCount);

            return result;
        }

        public async Task<ResponseList<OrderResponse>> GetPageOfOrdersByCustomerId(Guid id, int pageNumber, int pageSize)
        {
            List<OrderResponse> response = await _context.Orders.Include(x => x.Customer)
                                                                .Where(x => x.CustomerId == id)
                                                                .Skip((pageNumber - 1) * pageSize)
                                                                .Take(pageSize)
                                                                .Select(x => x.MapFromDb()).ToListAsync();

            int totalCount = await _context.Orders.CountAsync(x => x.CustomerId == id);

            ResponseList<OrderResponse> result = new(response, totalCount);

            return result;
        }

        public async Task<ResponseList<OrderResponse>> GetPageOfOrdersByStatus(string status, int pageNumber, int pageSize)
        {
            List<OrderResponse> response = await _context.Orders.Include(x => x.Customer)
                                                                .Where(x => x.OrderStatus == status)
                                                                .Skip((pageNumber - 1) * pageSize)
                                                                .Take(pageSize)
                                                                .Select(x => x.MapFromDb()).ToListAsync();

            int totalCount = await _context.Orders.CountAsync(x => x.OrderStatus == status);

            ResponseList<OrderResponse> result = new(response, totalCount);

            return result;
        }

        public async Task<OrderResponse?> GetBasketOrder(Guid customerId)
        {
            // поиск корзины (заказ со статусом basket)
            var ordersOfCustomer = _context.Orders.Where(x => x.CustomerId == customerId);

            if (!ordersOfCustomer.Any()) return null;

            Entity.Order? order = await ordersOfCustomer.FirstOrDefaultAsync(x => x.OrderStatus == "basket");

            if (order is null)
            {
                // создание при отсутствии
                order = new(customerId, _orderNumberGenerator.GenerateNewNumber);
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
            }

            OrderResponse orderResponse = order.MapFromDb();

            return orderResponse;
        }
    }
}
