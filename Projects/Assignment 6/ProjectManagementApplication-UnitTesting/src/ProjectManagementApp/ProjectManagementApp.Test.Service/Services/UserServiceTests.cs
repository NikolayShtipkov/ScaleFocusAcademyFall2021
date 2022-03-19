using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Data.Database;
using ProjectManagementApp.Data.Entities;
using ProjectManagementApp.Interfaces;
using ProjectManagementApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProjectManagementApp.Test.Service.Services
{
    public class UserServiceTests : ServiceTestBase
    {
        [Fact]
        public async Task Login_ValidInput_ReturnsUser()
        {
            await _userService.CreateUserAsync("test", "testpass", "testcho", "testkov", true, 0);

            var result = _userService.LogIn("test", "testpass");

            Assert.IsType<User>(result);

            await _context.Database.EnsureDeletedAsync();
        }

        [Fact]
        public void Login_InvalidInput_ReturnsNull()
        {
            var result = _userService.LogIn("fake", "testpass");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsList()
        {
            Assert.IsType<List<User>>(await _userService.GetAllUsersAsync());
        }

        [Fact]
        public async Task GetAllUsers_NotEmpty()
        {
            await _userService.CreateUserAsync("test", "testpass", "testcho", "testkov", true, 0);

            Assert.NotEmpty(await _userService.GetAllUsersAsync());

            await _context.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task GetUserById_ValidId_ReturnsUser()
        {
            await _userService.CreateUserAsync("test", "testpass", "testcho", "testkov", true, 0);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == "test");

            Assert.IsType<User>(await _userService.GetUserByIdAsync(user.Id));

            await _context.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task GetUserById_InvalidId_ReturnsNull()
        {
            Assert.Null(await _userService.GetUserByIdAsync(0));
        }

        [Fact]
        async public Task CreateUser_ValidInput_WithTeam_ReturnsTrue()
        {
            await _teamService.CreateTeamAsync("team");

            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Name == "team");
            var result = await _userService.CreateUserAsync("test", "testpass", "testcho", "testkov", true, team.Id);

            Assert.True(result);

            await _context.Database.EnsureDeletedAsync();
        }

        [Fact]
        async public Task CreateUser_ValidInput_WithoutTeam_ReturnsTrue()
        {
            var result = await _userService.CreateUserAsync("test", "testpass", "testcho", "testkov", true, 0);

            Assert.True(result);

            await _context.Database.EnsureDeletedAsync();
        }

        [Fact]
        async public Task CreateUser_InvalidUsername_ReturnsFalse()
        {
            await _userService.CreateUserAsync("test", "testpass", "testcho", "testkov", true, 0);

            var result = await _userService.CreateUserAsync("test", "testpass", "testcho", "testkov", true, 0);

            Assert.False(result);

            await _context.Database.EnsureDeletedAsync();
        }

        [Fact]
        async public Task EditUser_ValidInput_ReturnsTrue()
        {
            await _userService.CreateUserAsync("test", "testpass", "testcho", "testkov", true, 0);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == "test");
            var result = await _userService.EditUserAsync(user.Id, "test", "testpass", "testcho", "testkov");

            Assert.True(result);

            await _context.Database.EnsureDeletedAsync();
        }

        [Fact]
        async public Task EditUser_InvalidUsername_ReturnsFalse()
        {
            await _userService.CreateUserAsync("test", "testpass", "testcho", "testkov", true, 0);
            await _userService.CreateUserAsync("testMan", "testpass", "testcho", "testkov", true, 0);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == "test");
            var result = await _userService.EditUserAsync(user.Id, "testMan", "testpass", "testcho", "testkov");

            Assert.False(result);

            await _context.Database.EnsureDeletedAsync();
        }

        [Fact]
        async public Task DeleteUser_ValidInput_ReturnsTrue()
        {
            await _userService.CreateUserAsync("delete", "testpass", "testcho", "testkov", true, 0);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == "delete");
            var result = await _userService.DeleteUserAsync(user.Id);

            Assert.True(result);

            await _context.Database.EnsureDeletedAsync();
        }

        [Fact]
        async public Task DeleteUser_UserIsOwner_ReturnsFalse()
        {
            await _userService.CreateUserAsync("test", "testpass", "testcho", "testkov", true, 0);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == "test");

            await _projectService.CreateProjectAsync("project", user.Id);
            var result = await _userService.DeleteUserAsync(user.Id);

            Assert.False(result);

            await _context.Database.EnsureDeletedAsync();
        }
    }
}
