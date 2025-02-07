using OnlineStore.Server.DTO.User;

namespace OnlineStore.Server.Repositories.User
{
    public interface IUserRepository
    {
        Task<LoginResponse?> Authenticate(LoginRequest loginRequest);
        Task<bool> RegisterManager(ManagerRegisterRequest managerRegisterRequest);
        Task<bool> RegisterUser(Guid customerId, CustomerRegisterRequest customerRegisterRequest);
        Task<bool> UpdateUser(Guid id, UserRequest userRequest);
        Task<bool> DeleteUser(string name);
        Task<UserResponseList> GetPageOfUsersInfo(int pageNumber, int pageSize);
    }
}
