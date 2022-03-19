using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL.Entities;

namespace WorkforceManagementAPI.DAL.Contracts.IdentityContracts
{
    public interface IIdentityUserManager
    {
        Task<IdentityResult> AddUserToRoleAsync(User user, string password);
        Task<bool> VerifyEmail(string email);
        Task<IdentityResult> CreateUserAsync(User user, string password);
        Task<IdentityResult> DeleteUserAsync(User user);
        Task<List<User>> GetAllAsync();
        Task<User> GetUserAsync(ClaimsPrincipal principal);
        Task<List<string>> GetUserRolesAsync(User user);
        Task<bool> IsUserInRole(string userId, string roleName);
        Task<bool> ValidateUserCredentials(string userName, string password);
        Task<IdentityResult> UpdateUserDataAsync(User user);
        Task<User> FindByIdAsync(string id);
        Task<User> FindByNameAsync(string UserName);
    }
}