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
    public class CustomerRegistrationService : ICustomerRegistrationService, IDisposable
    {
        private readonly OnlineStoreDbContext _context;
        private readonly UserRepository _userRepository;
        private readonly CustomerRepository _customerRepository;
        private IDbContextTransaction? _transaction;
        private readonly ILogger<CustomerRegistrationService> _logger;
        private bool disposed = false;

        public CustomerRegistrationService(OnlineStoreDbContext context, IConfiguration configuration, ILogger<CustomerRegistrationService> logger)
        {
            _context = context;
            _userRepository = new UserRepository(_context, configuration);
            _customerRepository = new CustomerRepository(_context);
            _logger = logger;
        }
        public void Commit()
        {
            _transaction?.Commit();
        }

        public void CreateTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public async Task<bool> RegisterUser(RegisterRequest registerRequest)
        {
            if (registerRequest.CustomerRequest is null) return false; // регистрируем здесь только заказчиков

            // проверка данных заказчика
            bool isValid = CustomerValidator.CheckName(registerRequest.CustomerRequest.Name)
                        && CustomerValidator.CheckCode(registerRequest.CustomerRequest.Code)
                        && CustomerValidator.CheckDiscount(registerRequest.CustomerRequest.Discount);

            // создание заказчика, добавление в базу
            registerRequest.CustomerId = await _customerRepository.CreateCustomer(registerRequest.CustomerRequest);

            // проверка guid-а заказчика
            isValid &= CustomerValidator.CheckGuid(registerRequest.CustomerId);

            // выход (и отмена транзакции в вызывающем коде), если данные не валидны
            if (!isValid) return false;

            // проверка данных юзера
            isValid &= UserValidator.CheckUsername(registerRequest.Username)
                    && UserValidator.CheckPassword(registerRequest.Password)
                    && UserValidator.CheckGuid(registerRequest.CustomerId, registerRequest.Role);

            // добавление, если данные юзера валидны, и возврат результата добавления
            if (isValid)
            {
                return await _userRepository.RegisterUser(registerRequest);
            }

            // если не валидны - false (и отмена транзакции в вызывающем коде)
            return false;
        }

        public void Rollback()
        {
            _transaction?.RollbackAsync();
            _transaction?.Dispose();
        }

        public async Task Save()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка во время выполнения транзакции при попытке зарегистрировать пользователя (заказчика), метод Save().");
                throw new Exception(ex.Message, ex);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                _transaction?.Dispose();
            }
            disposed = true;
        }

        ~CustomerRegistrationService()
        {
            Dispose(false);
        }
    }
}
