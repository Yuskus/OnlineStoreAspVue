using OnlineStore.Server.DTO.User;

namespace OnlineStore.Server.Services.User.RegistrationService
{
    public interface ICustomerRegistrationService
    {
        Task<bool> RegisterUser(CustomerRegisterRequest customerRegisterRequest);
        void CreateTransaction();
        void Commit();
        void Rollback();
        Task Save();
    }
}
