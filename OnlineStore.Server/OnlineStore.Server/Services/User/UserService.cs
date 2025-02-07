using OnlineStore.Server.DTO.User;
using OnlineStore.Server.Repositories.User;
using OnlineStore.Server.Validation.User;

namespace OnlineStore.Server.Services.User
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<LoginResponse?> Authenticate(LoginRequest loginRequest)
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

        public async Task<UserResponseList> GetPageOfUsersInfo(int pageNumber, int pageSize)
        {
            bool isValid = UserValidator.CheckPages(pageNumber, pageSize);

            if (isValid)
            {
                return await _userRepository.GetPageOfUsersInfo(pageNumber, pageSize);
            }

            return new UserResponseList();
        }

        public async Task<bool> RegisterManager(ManagerRegisterRequest managerRegisterRequest)
        {
            bool isValid = UserValidator.CheckUsername(managerRegisterRequest.Username)
                        && UserValidator.CheckPassword(managerRegisterRequest.Password);

            // добавление, если данные юзера валидны, и возврат результата добавления
            if (isValid)
            {
                return await _userRepository.RegisterManager(managerRegisterRequest);
            }

            // если не валидны - false
            return false;
        }
    }
}
