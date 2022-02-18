using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Data.Database;
using ProjectManagementApp.Data.Entities;
using ProjectManagementApp.Interfaces;
using ProjectManagementApp.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.Services
{
    public class TeamService : ITeamService
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
            var team = await _context.Teams.FirstOrDefaultAsync(u => u.Id == id);
            if (team != null)
            {
                return team;
            }

            throw new NotFoundException($"Invalid input. Team with Id: { id } doesn't exist.");
        }

        public async Task<bool> CreateTeamAsync(string name)
        {
            if (await _context.Teams.AnyAsync(t => t.Name == name))
            {
                throw new AlreadyExistsException("Name is already in use.");
            }

            var team = new Team() { Name = name };

            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditTeamAsync(int id, string name)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == id);
            if (team == null)
            {
                throw new NotFoundException($"Invalid input. Team with Id: { id } doesn't exist.");
            }

            if (await _context.Teams.AnyAsync(t => t.Name == name) && team.Name != name)
            {
                throw new AlreadyExistsException("Name is already in use.");
            }

            team.Name = name;

            _context.Teams.Update(team);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteTeamAsync(int teamId)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == teamId);
            if (team == null)
            {
                throw new NotFoundException($"Invalid input. Team with Id: { teamId } doesn't exist.");
            }

            var teamUsers = await _context.TeamUser.Where(tu => tu.TeamId == teamId).ToListAsync();
            foreach (var tu in await _context.TeamUser.Where(tu => tu.TeamId == teamId).ToListAsync())
            {
                foreach (var task in await _context.Tasks.ToListAsync())
                {
                    if (tu.UserId == task.AssigneeId)
                    {
                        throw new DeleteRestrictedException("This team has a member with a task assigned to them and can't be deleted.");
                    }
                }
            }

            _context.Teams.Remove(team);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AssignUserToTeamAsync(int teamId, int userId)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == teamId);
            if (team == null)
            {
                throw new NotFoundException($"Invalid input. Team with Id: { teamId } doesn't exist.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new NotFoundException($"Invalid input. User with Id: { userId } doesn't exist.");
            }

            if (await _context.TeamUser.AnyAsync(tu => tu.TeamId == teamId && tu.UserId == userId))
            {
                throw new AlreadyExistsException($"User with Id: { userId } is already a member of team with Id: { teamId}.");
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
