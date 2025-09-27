using OnlineStore.Server.DTO.User;

namespace OnlineStore.Server.Services.User.RegistrationService
{
    public interface IRegistrationService
    {
        Task<bool> Register(CustomerRegisterRequest request);
        Task<bool> Register(UserCredentialsRequest request);
    }
}
