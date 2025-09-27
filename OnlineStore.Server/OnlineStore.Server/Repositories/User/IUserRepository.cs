using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.User;

namespace OnlineStore.Server.Repositories.User
{
    public interface IUserRepository
    {
        Task<LoginResponse?> Authenticate(UserCredentialsRequest loginRequest);
        Task<bool> CreateCustomerIfNotExists(CustomerRegisterRequest registerRequest);
        Task<bool> CreateUserIfNotExists(UserCredentialsRequest registerRequest);
        Task<bool> Update(string username, UserRequest userRequest);
        Task<bool> Delete(string name);
        Task<ResponseList<UserResponse>> GetAll();
    }
}
