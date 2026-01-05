using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Database.Context;
using OnlineStore.Server.Database.Entities;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Customers;
using OnlineStore.Server.Mapping.Customers;

namespace OnlineStore.Server.Repositories.Customers
{
    public class CustomerRepository(OnlineStoreDbContext context) : ICustomerRepository
    {
        private readonly OnlineStoreDbContext _context = context;

        public async Task<CustomerResponse?> Get(Guid id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);

            return customer?.MapFromDb();
        }

        public async Task<Guid?> CreateIfNotExists(CustomerBaseRequest customer)
        {
            Customer? customerEntity = await _context.Customers.FirstOrDefaultAsync(x => x.Code == customer.Code);

            if (customerEntity is null)
            {
                customerEntity = customer.MapToDb();

                await _context.Customers.AddAsync(customerEntity);
                await _context.SaveChangesAsync();
            }

            return customerEntity.Id;
        }

        public async Task<Guid?> CreateIfNotExists(CustomerRequest customer)
        {
            Customer? customerEntity = await _context.Customers.FirstOrDefaultAsync(x => x.Code == customer.Code);

            if (customerEntity is null)
            {
                customerEntity = customer.MapToDb();

                await _context.Customers.AddAsync(customerEntity);
                await _context.SaveChangesAsync();
            }

            return customerEntity.Id;
        }

        public async Task<bool> Update(Guid id, CustomerRequest customer)
        {
            Customer? customerEntity = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);

            if (customerEntity is null) return false;

            if (customerEntity.Code != customer.Code &&
                await _context.Customers.AnyAsync(x => x.Code == customer.Code))
            {
                return false;
            }

            customerEntity.Name = customer.Name;
            customerEntity.Code = customer.Code;
            customerEntity.Address = customer.Address;
            customerEntity.Discount = customer.Discount;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ResponseList<CustomerResponse>> GetPage(PageInfo pageInfo)
        {
            var query = _context.Customers
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
    }
}
