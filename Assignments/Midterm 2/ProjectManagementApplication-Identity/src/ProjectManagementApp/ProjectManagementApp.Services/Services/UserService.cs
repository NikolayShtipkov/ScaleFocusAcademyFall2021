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
    public class UserService : IUserService
    {
        private readonly DatabaseContext _context;
        private readonly TeamService _teamService;

        public UserService(DatabaseContext context)
        {
            _context = context;
            _teamService = new TeamService(context);
        }

        public User CurrentUser { get; private set; }

        public User LogIn(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                CurrentUser = user;
                return user;
            }

            throw new UnauthenticatedException("Invalid input. Incorrect username or password.");
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                return user;
            }

            throw new NotFoundException($"Invalid input. User with Id: { id } doesn't exist.");
        }

        public async Task<bool> CreateUserAsync(string username, string password, string firstName, string lastName, bool isAdmin, int teamId)
        {
            if (await _context.Users.AnyAsync(u => u.Username == username))
            {
                throw new AlreadyExistsException("Username is already in use.");
            }

            var team = await _context.Teams.FirstOrDefaultAsync(u => u.Id == teamId);
            if (teamId > 0 && team == null)
            {
                throw new NotFoundException("Invalid input, team Id doesn't exist.");
            }

            var user = new User()
            {
                Username = username,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                IsAdmin = isAdmin
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var newUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (teamId > 0)
            {
                await _teamService.AssignUserToTeamAsync(teamId, newUser.Id);
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditUserAsync(int id, string username, string password, string firstName, string lastName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new NotFoundException($"Invalid input. User with Id: { id } doesn't exist.");
            }

            if (await _context.Users.AnyAsync(u => u.Username == username) && user.Username != username)
            {
                throw new AlreadyExistsException("Username is already in use.");
            }

            user.Username = username;
            user.Password = password;
            user.FirstName = firstName;
            user.LastName = lastName;

            _context.Users.Update(user);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new NotFoundException($"Invalid input.User with Id: { id } doesn't exist.");
            }

            if (await _context.Projects.AnyAsync(p => p.OwnerId == id))
            {
                throw new DeleteRestrictedException("User owns one or more projects and can't be deleted.");
            }

            if (await IsAssigned(id))
            {
                throw new DeleteRestrictedException("User is assigned to 1 or more tasks in a project and can't be deleted.");
            }

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IsAssigned(int userId)
        {
            if (await _context.Tasks.AnyAsync(p => p.AssigneeId == userId))
            {
                return true;
            }

            return false;
        }
    }
}
