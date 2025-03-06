using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.User;

namespace OnlineStore.Server.Repositories.User
{
    public interface IUserRepository
    {
        Task<LoginResponse?> Authenticate(UserCredentialsRequest loginRequest);
        Task<bool> RegisterUser(UserCredentialsRequest registerRequest);
        Task<bool> UpdateUser(string username, UserRequest userRequest);
        Task<bool> DeleteUser(string name);
        Task<ResponseList<UserResponse>> GetAllUsers();
    }
}
