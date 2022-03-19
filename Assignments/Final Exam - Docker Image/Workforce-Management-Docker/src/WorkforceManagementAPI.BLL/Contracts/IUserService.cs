using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DTO.Models.Requests;

namespace WorkforceManagementAPI.BLL.Services
{
    public interface IUserService
    {
        Task<bool> CreateUser(CreateUserRequestDTO userRequest);

        Task<bool> DeleteUser(string id);

        Task<List<User>> GetAll();

        Task<User> GetCurrentUser(ClaimsPrincipal principal);

        Task<User> GetUserById(string id);

        Task<bool> UpdateUser(string userId,EditUserRequest editUserReaqest);

        Task SetAdministrator(string userId);
    }
}