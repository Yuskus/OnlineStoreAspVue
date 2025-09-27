using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Customer;

namespace OnlineStore.Server.Services.Customer
{
    public interface ICustomerService
    {
        Task<bool> Update(Guid id, CustomerRequest request);
        Task<ResponseList<CustomerResponse>> GetPage(int page, int pageSize);
        Task<CustomerResponse?> GetOneByCriteria(CustomerFilterCriteria criteria);
    }
}
