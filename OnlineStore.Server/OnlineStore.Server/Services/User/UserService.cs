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

        public async Task<bool> UpdateUser(UserRequest userRequest)
        {
            bool isValid = UserValidator.CheckGuid(userRequest.CustomerId, userRequest.Role)
                        && UserValidator.CheckUsername(userRequest.Username)
                        && UserValidator.CheckPassword(userRequest.Password);

            if (isValid)
            {
                return await _userRepository.UpdateUser(userRequest);
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

        public async Task<bool> AddManager(RegisterRequest registerRequest)
        {
            bool isValid = UserValidator.CheckUsername(registerRequest.Username)
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
    }
}
