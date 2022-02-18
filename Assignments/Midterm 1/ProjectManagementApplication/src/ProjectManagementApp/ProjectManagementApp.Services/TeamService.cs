using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Data.Database;
using ProjectManagementApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.Services
{
    public class TeamService
    {
        private readonly DatabaseContext _context;

        public TeamService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Team>> GetAllTeamsAsync()
        {
            return await _context.Teams.ToListAsync();
        }

        public async Task<Team> GetTeamByIdAsync(int id)
        {
            return await _context.Teams.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> CreateTeamAsync(string name)
        {
            if (await _context.Teams.AnyAsync(t => t.Name == name))
            {
                return false;
            }

            var team = new Team(){ Name = name} ;

            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditTeamAsync(int id, string name)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == id);
            if (await _context.Teams.AnyAsync(t => t.Name == name) && team.Name != name)
            {
                return false;
            }

            team.Name = name;

            _context.Teams.Update(team);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteTeamAsync(int teamId)
        {
            var teamUsers = await _context.TeamUser.Where(tu => tu.TeamId == teamId).ToListAsync();
            foreach (var tu in await _context.TeamUser.Where(tu => tu.TeamId == teamId).ToListAsync())
            {
                foreach (var task in await _context.Tasks.ToListAsync())
                {
                    if (tu.UserId == task.AssigneeId)
                    {
                        return false;
                    }
                }
            }

            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == teamId);

            _context.Teams.Remove(team);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AssignUserToTeamAsync(int teamId, int userId)
        {
            if (await _context.TeamUser.AnyAsync(tu => tu.TeamId == teamId && tu.UserId == userId))
            {
                return false;
            }

            var teamUser = new TeamUser()
            {
                TeamId = teamId,
                UserId = userId
            };

            await _context.TeamUser.AddAsync(teamUser);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<TeamUser>> GetTeamUsersAsync()
        {
            return await _context.TeamUser.ToListAsync();
        }
    }
}
