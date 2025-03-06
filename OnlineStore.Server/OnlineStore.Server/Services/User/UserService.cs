using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.User;
using OnlineStore.Server.Repositories.User;
using OnlineStore.Server.Validation.User;

namespace OnlineStore.Server.Services.User
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<LoginResponse?> Authenticate(UserCredentialsRequest loginRequest)
        {
            bool isValid = UserValidator.CheckUsername(loginRequest.Username)
                        && UserValidator.CheckPassword(loginRequest.Password);

            if (isValid)
            {
                return await _userRepository.Authenticate(loginRequest);
            }

            return null;
        }

        public async Task<bool> UpdateUser(string username, UserRequest userRequest)
        {
            bool isValid = UserValidator.CheckUsername(username)
                        && UserValidator.CheckUsername(userRequest.Username);

            if (isValid)
            {
                return await _userRepository.UpdateUser(username, userRequest);
            }

            return false;
        }

        public async Task<bool> DeleteUser(string name)
        {
            bool isValid = UserValidator.CheckUsername(name);

            if (isValid)
            {
                return await _userRepository.DeleteUser(name);
            }
            
            return false;
        }

        public async Task<ResponseList<UserResponse>> GetPageOfUsersInfo(int pageNumber, int pageSize)
        {
            bool isValid = UserValidator.CheckPages(pageNumber, pageSize);

            if (isValid)
            {
                return await _userRepository.GetPageOfUsersInfo(pageNumber, pageSize);
            }

            return new ResponseList<UserResponse>();
        }

        public async Task<bool> RegisterManager(UserCredentialsRequest registerRequest)
        {
            bool isValid = UserValidator.CheckUsername(registerRequest.Username)
                        && UserValidator.CheckPassword(registerRequest.Password);

            if (isValid)
            {
                return await _userRepository.RegisterUser(registerRequest);
            }

            return false;
        }
    }
}
