using OnlineStore.Server.DTO.User;

namespace OnlineStore.Server.Services.User
{
    public interface IUserService
    {
        Task<LoginResponse?> Authenticate(LoginRequest loginRequest);
        Task<bool> UpdateUser(UserRequest userRequest);
        Task<bool> DeleteUser(string name);
        Task<IEnumerable<UserResponse>> GetPageOfUsersInfo(int pageNumber, int pageSize); // add count() to response
    }
}
