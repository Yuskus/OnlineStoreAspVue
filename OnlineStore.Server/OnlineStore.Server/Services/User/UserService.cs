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
            bool isValid = UserValidator.CheckCredentials(loginRequest);

            if (isValid)
            {
                return await _userRepository.Authenticate(loginRequest);
            }

            return null;
        }

        public async Task<bool> Update(string username, UserRequest userRequest)
        {
            bool isValid = UserValidator.CheckUsername(username)
                        && UserValidator.CheckUsername(userRequest.Username);

            if (isValid)
            {
                return await _userRepository.Update(username, userRequest);
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

        public async Task<ResponseList<UserResponse>> GetPage(int pageNumber, int pageSize)
        {
            bool isValid = UserValidator.CheckPages(pageNumber, pageSize);

            if (isValid)
            {
                ResponseList<UserResponse> response = await _userRepository.GetAll();

                response.Responses = response.Responses.Skip((pageNumber - 1) * pageSize)
                                                       .Take(pageSize)
                                                       .ToList();

                return response;
            }

            return new ResponseList<UserResponse>();
        }
    }
}
