using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Customer;
using OnlineStore.Server.Repositories.Customer;
using OnlineStore.Server.Validation.Customer;

namespace OnlineStore.Server.Services.Customer
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

        public async Task<ResponseList<CustomerResponse>> GetPage(int page, int pageSize)
        {
            bool isValid = CustomerValidator.CheckPages(page, pageSize);

            if (isValid)
            {
                var response = await _customerRepository.GetAll();

                response.Responses = [.. response.Responses.Skip((page - 1) * pageSize).Take(pageSize)];

                return response;
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
