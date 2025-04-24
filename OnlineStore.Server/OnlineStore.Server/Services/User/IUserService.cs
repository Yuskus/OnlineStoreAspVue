using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.User;

namespace OnlineStore.Server.Services.User
{
    public interface IUserService
    {
        Task<LoginResponse?> Authenticate(UserCredentialsRequest loginRequest);
        Task<bool> Update(string username, UserRequest userRequest);
        Task<bool> Delete(string username);
        Task<ResponseList<UserResponse>> GetPage(int pageNumber, int pageSize);
    }
}
