using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Database.Context;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Customers;
using OnlineStore.Server.Mapping.Customers;
using OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Repositories.Customers
{
    public class CustomerRepository(OnlineStoreDbContext context) : ICustomerRepository
    {
        private readonly OnlineStoreDbContext _context = context;

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

            if (customerEntity.Code != customer.Code)
            {
                // на случай изменения кода новый код должен быть уникален
                if (await _context.Customers.AnyAsync(x => x.Code == customer.Code)) return false;
            }

            customerEntity.UpdateInDb(customer);
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

        public async Task<CustomerResponse?> GetOneByCriteria(CustomerFilterCriteria criteria)
        {
            Customer? result = null;

            if (criteria.Id is not null)
            {
                result = await _context.Customers.FirstOrDefaultAsync(x => x.Id == criteria.Id);
            }
            else if (criteria.Code is not null)
            {
                result = await _context.Customers.FirstOrDefaultAsync(x => x.Code == criteria.Code);
            }

            return result?.MapFromDb();
        }
    }
}
