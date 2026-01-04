using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Users;

namespace OnlineStore.Server.Repositories.Users
{
    public interface IUserRepository
    {
        Task<LoginResponse?> Authenticate(UserCredentialsRequest loginRequest);
        Task<bool> CreateCustomerIfNotExists(CustomerRegisterRequest registerRequest);
        Task<bool> CreateUserIfNotExists(UserCredentialsRequest registerRequest);
        Task<bool> Update(string username, UserRequest userRequest);
        Task<bool> Delete(string username);
        Task<ResponseList<UserResponse>> GetAll();
    }
}
