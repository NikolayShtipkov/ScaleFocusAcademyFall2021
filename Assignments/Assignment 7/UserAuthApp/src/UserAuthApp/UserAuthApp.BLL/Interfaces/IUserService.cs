using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using UserAuthApp.DAL.Entities;

namespace UserAuthApp.BLL.Interfaces
{
    public interface IUserService
    {
        Task<bool> CreateUser(string userName, string password);
        Task<List<User>> GetAll();
        Task<User> GetCurrentUser(ClaimsPrincipal principal);
        Task<User> GetUserById(string id);
        Task<User> GetUserByName(string name);
        Task<bool> IsUserInRole(string userId, string roleName);
    }
}