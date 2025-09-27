using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.User;

namespace OnlineStore.Server.Services.User
{
    public interface IUserService
    {
        Task<LoginResponse?> Authenticate(UserCredentialsRequest request);
        Task<bool> Update(string username, UserRequest request);
        Task<bool> Delete(string username);
        Task<ResponseList<UserResponse>> GetPage(int page, int pageSize);
    }
}
