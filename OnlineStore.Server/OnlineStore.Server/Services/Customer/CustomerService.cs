using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Customer;
using OnlineStore.Server.Repositories.Customer;
using OnlineStore.Server.Validation.Customer;

namespace OnlineStore.Server.Services.Customer
{
    public class CustomerService(ICustomerRepository customerRepository) : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository = customerRepository;

        public async Task<bool> UpdateCustomer(Guid id, CustomerRequest customer)
        {
            bool isValid = CustomerValidator.CheckGuid(id)
                        && CustomerValidator.CheckRequest(customer);

            if (isValid)
            {
                return await _customerRepository.UpdateCustomer(id, customer);
            }

            return false;
        }

        public async Task<ResponseList<CustomerResponse>> GetPageOfCustomers(int pageNumber, int pageSize)
        {
            bool isValid = CustomerValidator.CheckPages(pageNumber, pageSize);

            if (isValid)
            {
                var response = await _customerRepository.GetAllCustomers();

                response.Responses = response.Responses.Skip((pageNumber - 1) * pageSize)
                                                       .Take(pageSize)
                                                       .ToList();

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
