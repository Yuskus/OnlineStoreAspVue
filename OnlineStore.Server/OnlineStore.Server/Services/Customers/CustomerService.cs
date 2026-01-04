using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Customers;
using OnlineStore.Server.Repositories.Customers;
using OnlineStore.Server.Validation.Customers;

namespace OnlineStore.Server.Services.Customers
{
    public class CustomerService(ICustomerRepository customerRepository) : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository = customerRepository;

        public async Task<bool> Update(Guid id, CustomerRequest request)
        {
            bool isValid = CustomerValidator.CheckGuid(id)
                        && CustomerValidator.CheckRequest(request);

            if (isValid)
            {
                return await _customerRepository.Update(id, request);
            }

            return false;
        }

        public async Task<ResponseList<CustomerResponse>> GetPage(PageInfo pageInfo)
        {
            bool isValid = CustomerValidator.CheckPages(pageInfo.Number, pageInfo.Size);

            if (isValid)
            {
                return await _customerRepository.GetPage(pageInfo);
            }

            return new ResponseList<CustomerResponse>();
        }

        public async Task<CustomerResponse?> GetOneByCriteria(CustomerFilterCriteria criteria)
        {
            bool isValid = CustomerValidator.CheckCriteria(criteria);

            if (isValid)
            {
                return await _customerRepository.GetOneByCriteria(criteria);
            }

            return null;
        }
    }
}
