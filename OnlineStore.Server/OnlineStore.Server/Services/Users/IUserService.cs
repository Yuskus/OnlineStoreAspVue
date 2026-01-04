using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Users;

namespace OnlineStore.Server.Services.Users
{
    public interface IUserService
    {
        Task<bool> Register(CustomerRegisterRequest request);
        Task<bool> Register(UserCredentialsRequest request);
        Task<LoginResponse?> Authenticate(UserCredentialsRequest request);
        Task<bool> Update(string username, UserRequest request);
        Task<bool> Delete(string username);
        Task<ResponseList<UserResponse>> GetPage(int page, int pageSize);
    }
}
