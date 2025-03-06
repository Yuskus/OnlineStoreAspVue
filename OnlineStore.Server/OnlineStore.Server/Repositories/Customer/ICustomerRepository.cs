using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Customer;

namespace OnlineStore.Server.Repositories.Customer
{
    public interface ICustomerRepository
    {
        Task<ResponseList<CustomerResponse>> GetAllCustomers();
        Task<CustomerResponse?> GetOneByCriteria(CustomerFilterCriteria criteria);
        Task<Guid?> CreateCustomer(CustomerBaseRequest customer);
        Task<bool> UpdateCustomer(Guid id, CustomerRequest customer);
    }
}
