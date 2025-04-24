using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OnlineStore.Server.Database.Context;
using OnlineStore.Server.DTO.User;
using OnlineStore.Server.Repositories.Customer;
using OnlineStore.Server.Repositories.User;
using OnlineStore.Server.Validation.Customer;
using OnlineStore.Server.Validation.User;

namespace OnlineStore.Server.Services.User.RegistrationService
{
    public class CustomerRegistrationService : IRegistrationService<CustomerRegisterRequest>
    {
        private readonly OnlineStoreDbContext _context;
        private readonly UserRepository _userRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly ILogger<CustomerRegistrationService> _logger;

        public CustomerRegistrationService(OnlineStoreDbContext context, IConfiguration configuration, ILogger<CustomerRegistrationService> loggerCustomer, ILogger<UserRepository> loggerUser)
        {
            _context = context;
            _userRepository = new UserRepository(_context, configuration, loggerUser);
            _customerRepository = new CustomerRepository(_context);
            _logger = loggerCustomer;
        }

        public async Task<bool> Register(CustomerRegisterRequest customerRegisterRequest)
        {
            IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                bool isValid = CustomerValidator.CheckRequest(customerRegisterRequest.CustomerInfo);

                if (!isValid) return false;

                customerRegisterRequest.Id = await _customerRepository.Create(customerRegisterRequest.CustomerInfo);

                isValid &= CustomerValidator.CheckGuid(customerRegisterRequest.Id)
                        && UserValidator.CheckCredentials(customerRegisterRequest);

                if (isValid && await _userRepository.Create(customerRegisterRequest) is bool result)
                {
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return result;
                }

                await transaction.RollbackAsync();
                return false;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка во время выполнения транзакции при попытке зарегистрировать пользователя (заказчика), метод Save().");
                await transaction.RollbackAsync();
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе RegisterCustomer.");
                await transaction.RollbackAsync();
                return false;
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        }
    }
}
