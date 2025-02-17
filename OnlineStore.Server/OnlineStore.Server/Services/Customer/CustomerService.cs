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
                        && CustomerValidator.CheckName(customer.Name)
                        && CustomerValidator.CheckCode(customer.Code)
                        && CustomerValidator.CheckDiscount(customer.Discount);

            if (isValid)
            {
                return await _customerRepository.UpdateCustomer(id, customer);
            }

            return false;
        }

        public async Task<CustomerResponse?> GetCustomerByCode(string code)
        {
            bool isValid = CustomerValidator.CheckCode(code);

            if (isValid)
            {
                return await _customerRepository.GetCustomerByCode(code);
            }
            
            return null;
        }

        public async Task<CustomerResponse?> GetCustomerById(Guid id)
        {
            bool isValid = CustomerValidator.CheckGuid(id);

            if (isValid)
            {
                return await _customerRepository.GetCustomerById(id);
            }

            return null;
        }

        public async Task<ResponseList<CustomerResponse>> GetPageOfCustomers(int pageNumber, int pageSize)
        {
            bool isValid = CustomerValidator.CheckPages(pageNumber, pageSize);

            if (isValid)
            {
                return await _customerRepository.GetPageOfCustomers(pageNumber, pageSize);
            }

            return new ResponseList<CustomerResponse>();
        }
    }
}
