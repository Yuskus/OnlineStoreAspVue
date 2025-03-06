using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Customer;

namespace OnlineStore.Server.Services.Customer
{
    public interface ICustomerService
    {
        Task<bool> Update(Guid id, CustomerRequest customer);
        Task<ResponseList<CustomerResponse>> GetPage(int pageNumber, int pageSize);
        Task<CustomerResponse?> GetOneByCriteria(CustomerFilterCriteria criteria);
    }
}
