using OnlineStore.Server.DTO.Customer;

namespace OnlineStore.Server.Services.Customer
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerResponse>> GetPageOfCustomers(int pageNumber, int pageSize); // add count() to response
        Task<CustomerResponse?> GetCustomerById(Guid id);
        Task<CustomerResponse?> GetCustomerByCode(string code);
        Task<Guid?> CreateCustomer(CustomerRequest customer);
        Task<bool> UpdateCustomer(Guid id, CustomerRequest customer);
        Task<bool> DeleteCustomer(Guid id);
    }
}
