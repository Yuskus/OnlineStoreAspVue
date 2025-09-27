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

        public async Task<bool> Register(CustomerRegisterRequest request)
        {
            await _transactionService.BeginTransactionAsync();

            try
            {
                bool isValidCustomer = CustomerValidator.CheckRequest(request.CustomerInfo);

                if (!isValidCustomer) return false;

                request.Id = await _customerRepository.CreateIfNotExists(request.CustomerInfo);

                bool isValidUser =
                    CustomerValidator.CheckGuid(request.Id) &&
                    UserValidator.CheckCredentials(request.Username, request.Password);

                if (isValidUser)
                {
                    bool result = await _userRepository.CreateCustomerIfNotExists(request);

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
