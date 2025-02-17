using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.User;

namespace OnlineStore.Server.Repositories.User
{
    public interface IUserRepository
    {
        Task<LoginResponse?> Authenticate(LoginRequest loginRequest);
        Task<bool> RegisterManager(ManagerRegisterRequest managerRegisterRequest);
        Task<bool> RegisterUser(Guid customerId, CustomerRegisterRequest customerRegisterRequest);
        Task<bool> UpdateUser(string username, UserRequest userRequest);
        Task<bool> DeleteUser(string name);
        Task<ResponseList<UserResponse>> GetPageOfUsersInfo(int pageNumber, int pageSize);
    }
}
