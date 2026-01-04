using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Customers;

namespace OnlineStore.Server.Services.Customers
{
    public interface ICustomerService
    {
        Task<bool> Update(Guid id, CustomerRequest request);
        Task<ResponseList<CustomerResponse>> GetPage(int page, int pageSize);
        Task<CustomerResponse?> GetOneByCriteria(CustomerFilterCriteria criteria);
    }
}
