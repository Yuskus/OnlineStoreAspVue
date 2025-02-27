﻿using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.User;

namespace OnlineStore.Server.Services.User
{
    public interface IUserService
    {
        Task<LoginResponse?> Authenticate(LoginRequest loginRequest);
        Task<bool> RegisterManager(ManagerRegisterRequest managerRegisterRequest);
        Task<bool> UpdateUser(string username, UserRequest userRequest);
        Task<bool> DeleteUser(string name);
        Task<ResponseList<UserResponse>> GetPageOfUsersInfo(int pageNumber, int pageSize);
    }
}
