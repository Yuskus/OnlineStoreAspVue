using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Database.Context;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Customer;
using OnlineStore.Server.Mapping.Customer;
using Entity = OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Repositories.Customer
{
    public class CustomerRepository(OnlineStoreDbContext context) : ICustomerRepository
    {
        private readonly OnlineStoreDbContext _context = context;

        public async Task<Guid?> CreateCustomer(CustomerBaseRequest customer)
        {
            Entity.Customer? customerEntity = await _context.Customers.FirstOrDefaultAsync(x => x.Code == customer.Code);

            if (customerEntity is null)
            {
                customerEntity = customer.MapToDb();

                await _context.Customers.AddAsync(customerEntity);
                await _context.SaveChangesAsync();
            }

            return customerEntity.Id;
        }

        public async Task<bool> UpdateCustomer(Guid id, CustomerRequest customer)
        {
            Entity.Customer? customerEntity = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);

            if (customerEntity is null) return false;

            customerEntity.UpdateInDb(customer);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<ResponseList<CustomerResponse>> GetAllCustomers()
        {
            return new()
            {
                Responses = await _context.Customers.Select(x => x.MapFromDb()).ToListAsync(),
                TotalCount = await _context.Customers.CountAsync()
            };
        }

        public async Task<CustomerResponse?> GetOneByCriteria(CustomerFilterCriteria criteria)
        {
            Entity.Customer? result = null;

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
