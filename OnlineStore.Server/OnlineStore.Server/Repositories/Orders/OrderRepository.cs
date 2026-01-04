using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Database.Context;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Orders;
using OnlineStore.Server.Mapping.Orders;
using OnlineStore.Server.Utilities.Order.Generators;
using OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Repositories.Orders
{
    public class OrderRepository(OnlineStoreDbContext context, INumberGenerator orderNumberGenerator) : IOrderRepository
    {
        private readonly OnlineStoreDbContext _context = context;
        private readonly INumberGenerator _orderNumberGenerator = orderNumberGenerator;

        public async Task<Guid?> Create(OrderRequest order)
        {
            if (await _context.Orders.AnyAsync(x => x.CustomerId == order.CustomerId) == false) return null;

            Order orderEntity = order.MapToDb();

            await _context.Orders.AddAsync(orderEntity);
            orderEntity.OrderNumber = _orderNumberGenerator.GenerateNewNumber;
            await _context.SaveChangesAsync();

            return orderEntity.Id;
        }

        public async Task<bool> Update(Guid orderId, OrderRequest order)
        {
            if (await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId) is Order orderEntity)
            {
                if (await _context.Customers.AnyAsync(x => x.Id == order.CustomerId))
                {
                    orderEntity.UpdateInDb(order);
                    await _context.SaveChangesAsync();

                    return true;
                }
            }

            return false;
        }

        public async Task<bool> Delete(Guid orderId)
        {
            if (await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId) is Order orderEntity)
            {
                _context.Orders.Remove(orderEntity);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<ResponseList<OrderResponse>> GetPage(PageInfo pageInfo)
        {
            IQueryable<Order> query = _context.Orders
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

        public async Task<ResponseList<OrderResponse>> GetPageByCriteria(OrderFilterCriteria criteria, PageInfo pageInfo)
        {
            IQueryable<Order> filtredQuery = FilteringOrders(criteria);

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

        public async Task<OrderResponse?> GetOneByCriteria(OrderFilterCriteria criteria)
        {
            IQueryable<Order> filtredQuery = FilteringOrders(criteria);

            return await filtredQuery
                .Select(x => x.MapFromDb())
                .FirstOrDefaultAsync();
        }

        private IQueryable<Order> FilteringOrders(OrderFilterCriteria criteria)
        {
            IQueryable<Order> orders = _context.Orders
                .Include(x => x.Customer)
                .AsSingleQuery();

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

            return orders;
        }
    }
}
