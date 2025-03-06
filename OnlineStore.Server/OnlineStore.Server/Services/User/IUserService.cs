using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.User;

namespace OnlineStore.Server.Services.User
{
    public interface IUserService
    {
        Task<LoginResponse?> Authenticate(UserCredentialsRequest loginRequest);
        Task<bool> RegisterManager(UserCredentialsRequest registerRequest);
        Task<bool> UpdateUser(string username, UserRequest userRequest);
        Task<bool> DeleteUser(string name);
        Task<ResponseList<UserResponse>> GetPageOfUsersInfo(int pageNumber, int pageSize);
    }
}
