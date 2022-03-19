using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL.Contracts;
using WorkforceManagementAPI.DAL.Entities;

namespace WorkforceManagementAPI.DAL.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly DatabaseContext _context;

        public TeamRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Team>> GetAllTeamsAsync()
        {
            return await _context.Teams.ToListAsync();
        }
        public async Task<List<Team>> GetMyTeamsAsync(string userId)
        {
            var teams = await _context.Teams
                .Where(t => t.Users
                    .Any(u => u.Id.Equals(userId)))
                .ToListAsync();

            return teams;
        }

        public async Task<Team> GetTeamByIdAsync(Guid teamId)
        {
            return await _context.Teams.FirstOrDefaultAsync(t => t.Id == teamId);
        }

        public async Task AddTeamAsync(Team team)
        {
            await _context.Teams.AddAsync(team);
        }

        public void UpdateTeam(Team team)
        {
            _context.Teams.Update(team);
        }

        public void RemoveTeam(Team team)
        {
            _context.Teams.Remove(team);
        }

        public void AddTeamUser(Team team, User user)
        {
            team.Users.Add(user);
            user.Teams.Add(team);
        }

        public void RemoveTeamUser(Team team, User user)
        {
            team.Users.Remove(user);
            user.Teams.Remove(team);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
