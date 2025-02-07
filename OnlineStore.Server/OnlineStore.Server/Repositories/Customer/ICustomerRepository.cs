using OnlineStore.Server.DTO.Customer;

namespace OnlineStore.Server.Repositories.Customer
{
    public interface ICustomerRepository
    {
        Task<CustomerResponseList> GetPageOfCustomers(int pageNumber, int pageSize);
        Task<CustomerResponse?> GetCustomerById(Guid id);
        Task<CustomerResponse?> GetCustomerByCode(string code);
        Task<Guid?> CreateCustomer(CustomerBaseRequest customer);
        Task<bool> UpdateCustomer(Guid id, CustomerRequest customer);
        Task<bool> DeleteCustomer(Guid id);
    }
}
