using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.User;
using OnlineStore.Server.Repositories.User;
using OnlineStore.Server.Validation.User;

namespace OnlineStore.Server.Services.User
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<LoginResponse?> Authenticate(UserCredentialsRequest request)
        {
            bool isValid = UserValidator.CheckCredentials(request);

            if (isValid)
            {
                return await _userRepository.Authenticate(request);
            }

            return null;
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
