using OnlineStore.Server.DTO.User;

namespace OnlineStore.Server.Services.User
{
    public interface IUserService
    {
        Task<LoginResponse?> Authenticate(LoginRequest loginRequest);
        Task<bool> RegisterManager(ManagerRegisterRequest managerRegisterRequest);
        Task<bool> UpdateUser(Guid id, UserRequest userRequest);
        Task<bool> DeleteUser(string name);
        Task<UserResponseList> GetPageOfUsersInfo(int pageNumber, int pageSize);
    }
}
