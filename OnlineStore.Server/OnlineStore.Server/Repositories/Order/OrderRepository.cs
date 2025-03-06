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

        public async Task<Guid?> Create(OrderRequest order)
        {
            Entity.Order orderEntity = order.MapToDb();

            await _context.Orders.AddAsync(orderEntity);
            orderEntity.OrderNumber = _orderNumberGenerator.GenerateNewNumber;
            await _context.SaveChangesAsync();

            return orderEntity.Id;
        }

        public async Task<bool> Update(Guid id, OrderRequest order)
        {
            if (await _context.Orders.FirstOrDefaultAsync(x => x.Id == id) is Entity.Order orderEntity)
            {
                orderEntity.UpdateInDb(order);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            if (await _context.Orders.FirstOrDefaultAsync(x => x.Id == id) is Entity.Order orderEntity)
            {
                _context.Orders.Remove(orderEntity);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<ResponseList<OrderResponse>> GetAll()
        {
            return new()
            {
                Responses = await _context.Orders.Select(x => x.MapFromDb()).ToListAsync(),
                TotalCount = await _context.Orders.CountAsync()
            };
        }

        public async Task<ResponseList<OrderResponse>> GetAllByCriteria(OrderFilterCriteria criteria)
        {
            IEnumerable<OrderResponse> filtred = await FilteringOrders(criteria);

            return new()
            {
                Responses = filtred,
                TotalCount = filtred.Count()
            };
        }

        public async Task<OrderResponse?> GetOneByCriteria(OrderFilterCriteria criteria)
        {
            IEnumerable<OrderResponse> filtred = await FilteringOrders(criteria);

            return filtred.FirstOrDefault();
        }

        private async Task<IEnumerable<OrderResponse>> FilteringOrders(OrderFilterCriteria criteria)
        {
            IQueryable<Entity.Order> orders = _context.Orders;

            if (criteria.Id is not null)
            {
                orders = orders.Where(x => x.Id == criteria.Id);
            }
            if (criteria.OrderNumber is not null)
            {
                orders = orders.Where(x => x.OrderNumber == criteria.OrderNumber);
            }
            if (criteria.CustomerId is not null)
            {
                orders = orders.Where(x => x.CustomerId == criteria.CustomerId);
            }
            if (criteria.OrderStatus is not null)
            {
                orders = orders.Where(x => x.OrderStatus == criteria.OrderStatus);
            }

            return await orders.Include(x => x.Customer)
                               .Select(x => x.MapFromDb())
                               .ToListAsync(); //возвращает ли customer name?
        }
    }
}
