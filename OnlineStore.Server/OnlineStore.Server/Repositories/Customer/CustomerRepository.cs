﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<CustomerResponse?> GetCustomerByCode(string code)
        {
            Entity.Customer? customerEntity = await _context.Customers.FirstOrDefaultAsync(x => x.Code == code);

            if (customerEntity is null) return null;

            CustomerResponse result = customerEntity.MapFromDb();

            return result;
        }

        public async Task<CustomerResponse?> GetCustomerById(Guid id)
        {
            Entity.Customer? customerEntity = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);

            if (customerEntity is null) return null;

            CustomerResponse result = customerEntity.MapFromDb();

            return result;
        }

        public async Task<ResponseList<CustomerResponse>> GetPageOfCustomers(int pageNumber, int pageSize)
        {
            List<CustomerResponse> response = await _context.Customers.Skip((pageNumber - 1) * pageSize)
                                                                      .Take(pageSize)
                                                                      .Select(x => x.MapFromDb()).ToListAsync();
            int totalCount = await _context.Customers.CountAsync();

            ResponseList<CustomerResponse> result = new(response, totalCount);

            return result;
        }
    }
}
