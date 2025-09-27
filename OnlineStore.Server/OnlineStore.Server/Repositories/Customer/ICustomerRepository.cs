using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Customer;

namespace OnlineStore.Server.Repositories.Customer
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
