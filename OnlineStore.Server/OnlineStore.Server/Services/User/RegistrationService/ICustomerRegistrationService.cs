using OnlineStore.Server.DTO.User;

namespace OnlineStore.Server.Services.User.RegistrationService
{
    public interface ICustomerRegistrationService
    {
        Task<bool> RegisterUser(RegisterRequest registerRequest);
        void CreateTransaction();
        void Commit();
        void Rollback();
        Task Save();
    }
}
