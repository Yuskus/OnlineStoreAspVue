using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Customers;

namespace OnlineStore.Server.Repositories.Customers
{
    public interface ICustomerRepository
    {
        Task<Guid?> CreateIfNotExists(CustomerBaseRequest customer);
        Task<Guid?> CreateIfNotExists(CustomerRequest customer);
        Task<bool> Update(Guid id, CustomerRequest customer);
        Task<ResponseList<CustomerResponse>> GetAll();
        Task<CustomerResponse?> GetOneByCriteria(CustomerFilterCriteria criteria);
    }
}
