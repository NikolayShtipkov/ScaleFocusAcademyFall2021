using ProjectManagementApp.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementApp.Interfaces
{
    public interface IUserService
    {
        User CurrentUser { get; }

        Task<bool> CreateUserAsync(string username, string password, string firstName, string lastName, bool isAdmin, int teamId);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> EditUserAsync(int id, string username, string password, string firstName, string lastName);
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<bool> IsAssigned(int userId);
        User LogIn(string username, string password);
    }
}