using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Customer;

namespace OnlineStore.Server.Services.Customer
{
    public interface ICustomerService
    {
        Task<ResponseList<CustomerResponse>> GetPageOfCustomers(int pageNumber, int pageSize);
        Task<CustomerResponse?> GetOneByCriteria(CustomerFilterCriteria criteria);
        Task<bool> UpdateCustomer(Guid id, CustomerRequest customer);
    }
}
