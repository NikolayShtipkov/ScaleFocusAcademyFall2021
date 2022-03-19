using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL.Entities;

namespace WorkforceManagementAPI.DAL.Contracts
{
    public interface ITeamRepository
    {
        Task AddTeamAsync(Team team);
        void AddTeamUser(Team team, User user);
        Task<List<Team>> GetAllTeamsAsync();
        Task<List<Team>> GetMyTeamsAsync(string userId);
        Task<Team> GetTeamByIdAsync(Guid teamId);
        void RemoveTeam(Team team);
        void RemoveTeamUser(Team team, User user);
        Task SaveChangesAsync();
        void UpdateTeam(Team team);
    }
}