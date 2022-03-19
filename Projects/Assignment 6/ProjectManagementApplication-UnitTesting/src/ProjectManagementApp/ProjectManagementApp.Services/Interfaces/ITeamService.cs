using ProjectManagementApp.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementApp.Interfaces
{
    public interface ITeamService
    {
        Task<bool> AssignUserToTeamAsync(int teamId, int userId);
        Task<bool> CreateTeamAsync(string name);
        Task<bool> DeleteTeamAsync(int teamId);
        Task<bool> EditTeamAsync(int id, string name);
        Task<List<Team>> GetAllTeamsAsync();
        Task<Team> GetTeamByIdAsync(int id);
        Task<List<TeamUser>> GetTeamUsersAsync();
    }
}