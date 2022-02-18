using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DTO.Models.Requests;

namespace WorkforceManagementAPI.BLL.Services
{
    public interface IUserService
    {
        Task<bool> CreateUserAsync(CreateUserRequestDTO userRequest);

        Task<bool> DeleteUserAsync(string id);

        Task<List<User>> GetAllUsersAsync();

        Task<User> GetCurrentUser(ClaimsPrincipal principal);

        Task<User> GetUserByIdAsync(string id);

        Task<bool> UpdateUserAsync(string userId,EditUserRequest editUserReaqest);

        Task SetAdministratorAsync(string userId);
    }
}