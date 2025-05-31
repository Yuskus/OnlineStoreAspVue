using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.DTO.User;
using OnlineStore.Server.Repositories.Customer;
using OnlineStore.Server.Repositories.User;
using OnlineStore.Server.Utilities.Common.Database;
using OnlineStore.Server.Validation.Customer;
using OnlineStore.Server.Validation.User;

namespace OnlineStore.Server.Services.User.RegistrationService
{
    public class CustomerRegistrationService : IRegistrationService<CustomerRegisterRequest>
    {
        private readonly ITransactionService _transactionService;
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CustomerRegistrationService> _logger;

        public CustomerRegistrationService(ITransactionService transactionService,
                                           IUserRepository userRepository,
                                           ICustomerRepository customerRepository,
                                           ILogger<CustomerRegistrationService> logger)
        {
            _transactionService = transactionService;
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _logger = logger;
        }

        public async Task<bool> Register(CustomerRegisterRequest customerRegisterRequest)
        {
            await _transactionService.BeginTransactionAsync();

            try
            {
                bool isValidCustomer = CustomerValidator.CheckRequest(customerRegisterRequest.CustomerInfo);

                if (!isValidCustomer) return false;

                customerRegisterRequest.Id = await _customerRepository.Create(customerRegisterRequest.CustomerInfo);

                bool isValidUser = CustomerValidator.CheckGuid(customerRegisterRequest.Id)
                                && UserValidator.CheckCredentials(customerRegisterRequest);

                if (isValidUser)
                {
                    bool result = await _userRepository.Create(customerRegisterRequest);

                    await _transactionService.SaveChangesAsync();
                    await _transactionService.CommitAsync();

                    return result;
                }
            }
            catch (DbUpdateException ex)
            {
                await _transactionService.RollbackAsync();
                _logger.LogError(ex, "Ошибка во время выполнения транзакции при попытке зарегистрировать пользователя (заказчика), метод Save().");
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();
                _logger.LogError(ex, "Ошибка при запросе RegisterCustomer.");
            }
            finally
            {
                await _transactionService.DisposeAsync();
            }

            return false;
        }
    }
}
