using OnlineStore.Server.DTO.User;
using OnlineStore.Server.Repositories.User;
using OnlineStore.Server.Validation.User;

namespace OnlineStore.Server.Services.User.RegistrationService
{
    public class ManagerRegistrationService(IUserRepository userRepository) : IRegistrationService<UserCredentialsRequest>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<bool> Register(UserCredentialsRequest registerRequest)
        {
            bool isValid = UserValidator.CheckCredentials(registerRequest);

            if (isValid)
            {
                return await _userRepository.CreateUserIfNotExists(registerRequest);
            }

            return false;
        }
    }
}
