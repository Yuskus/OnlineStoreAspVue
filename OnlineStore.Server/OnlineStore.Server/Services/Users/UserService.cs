using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Users;
using OnlineStore.Server.Repositories.Customers;
using OnlineStore.Server.Repositories.Users;
using OnlineStore.Server.Utilities.Common.Database;
using OnlineStore.Server.Validation.Customers;
using OnlineStore.Server.Validation.Users;

namespace OnlineStore.Server.Services.Users
{
    public class UserService(
        IUserRepository userRepository,
        ICustomerRepository customerRepository,
        ITransactionService transactionService,
        ILogger<UserService> logger) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ICustomerRepository _customerRepository = customerRepository;
        private readonly ITransactionService _transactionService = transactionService;
        private readonly ILogger<UserService> _logger = logger;

        public async Task<LoginResponse?> Authenticate(UserCredentialsRequest request)
        {
            bool isValid = UserValidator.CheckCredentials(request);

            if (isValid)
            {
                return await _userRepository.Authenticate(request);
            }

            return null;
        }

        public async Task<bool> Register(UserCredentialsRequest request)
        {
            bool isValid = UserValidator.CheckCredentials(request);

            if (isValid)
            {
                return await _userRepository.CreateUserIfNotExists(request);
            }

            return false;
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

        public async Task<bool> Update(string username, UserRequest request)
        {
            bool isValid = UserValidator.CheckUsername(username)
                        && UserValidator.CheckUsername(request.Username);

            if (isValid)
            {
                return await _userRepository.Update(username, request);
            }

            return false;
        }

        public async Task<bool> Delete(string username)
        {
            bool isValid = UserValidator.CheckUsername(username);

            if (isValid)
            {
                return await _userRepository.Delete(username);
            }
            
            return false;
        }

        public async Task<ResponseList<UserResponse>> GetPage(int page, int pageSize)
        {
            bool isValid = UserValidator.CheckPages(page, pageSize);

            if (isValid)
            {
                ResponseList<UserResponse> response = await _userRepository.GetAll();

                response.Responses = [.. response.Responses.Skip((page - 1) * pageSize).Take(pageSize)];

                return response;
            }

            return new ResponseList<UserResponse>();
        }
    }
}
