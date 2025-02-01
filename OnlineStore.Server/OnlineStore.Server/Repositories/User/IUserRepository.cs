using OnlineStore.Server.DTO.User;

namespace OnlineStore.Server.Repositories.User
{
    public interface IUserRepository
    {
        Task<LoginResponse?> Authenticate(LoginRequest loginRequest);
        Task<bool> RegisterUser(RegisterRequest registerRequest);
        Task<bool> UpdateUser(UserRequest userRequest);
        Task<bool> DeleteUser(string name);
        Task<UserResponseList> GetPageOfUsersInfo(int pageNumber, int pageSize);
    }
}
