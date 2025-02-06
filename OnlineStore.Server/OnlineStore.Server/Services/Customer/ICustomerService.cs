﻿using OnlineStore.Server.DTO.Customer;

namespace OnlineStore.Server.Services.Customer
{
    public interface ICustomerService
    {
        Task<CustomerResponseList> GetPageOfCustomers(int pageNumber, int pageSize);
        Task<CustomerResponse?> GetCustomerById(Guid id);
        Task<CustomerResponse?> GetCustomerByCode(string code);
        Task<bool> UpdateCustomer(Guid id, CustomerRequest customer);
        Task<bool> DeleteCustomer(Guid id);
    }
}
