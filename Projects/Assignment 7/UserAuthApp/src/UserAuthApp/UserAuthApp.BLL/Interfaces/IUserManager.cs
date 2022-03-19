using UserAuthApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthApp.BLL.Interfaces
{
    public interface IUserManager
    {
        Task<User> GetUserAsync(ClaimsPrincipal claimsPrincipal);
        Task<bool> IsUserInRole(string userId, string roleId);
        Task<User> FindByNameAsync(string name);
        Task<User> FindByIdAsync(string id);
        Task<List<User>> GetAllAsync();
        Task CreateUserAsync(User user, string password);

        Task<List<string>> GetUserRolesAsync(User user);
        Task<bool> ValidateUserCredentials(string userName, string password);
    }
}
