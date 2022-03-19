using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Data.Database;
using ProjectManagementApp.Data.Entities;
using ProjectManagementApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProjectManagementApp.Test.Service.Services
{
    public class TeamServiceTests
    {
        [Fact]
        public async Task GetAllTeams_ReturnsList()
        {
            var context = SetTeamDatabase();
            var teamService = new TeamService(context);

            Assert.IsType<List<Team>>(await teamService.GetAllTeamsAsync());
        }

        [Fact]
        public async Task GetAllTeams_NotEmpty()
        {
            var context = SetTeamDatabase();
            var teamService = new TeamService(context);

            await teamService.CreateTeamAsync("test");

            Assert.NotEmpty(await teamService.GetAllTeamsAsync());

            await context.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task GetTeamById_ValidInput_ReturnsUser()
        {
            var context = SetTeamDatabase();
            var teamService = new TeamService(context);

            await teamService.CreateTeamAsync("test");
            var team = await context.Teams.FirstOrDefaultAsync(t => t.Name == "test");

            Assert.IsType<Team>(await teamService.GetTeamByIdAsync(team.Id));

            await context.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task GetTeamById_InvalidId_ReturnsNull()
        {
            var context = SetTeamDatabase();
            var teamService = new TeamService(context);

            Assert.Null(await teamService.GetTeamByIdAsync(0));
        }

        [Fact]
        public async Task CreateTeam_ValidInput_ReturnsTrue()
        {
            var context = SetTeamDatabase();
            var teamService = new TeamService(context);

            var result = await teamService.CreateTeamAsync("test");

            Assert.True(result);

            await context.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task CreateTeam_InvalidName_ReturnsFalse()
        {
            var context = SetTeamDatabase();
            var teamService = new TeamService(context);

            await teamService.CreateTeamAsync("test");

            var result = await teamService.CreateTeamAsync("test");

            Assert.False(result);

            await context.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task EditTeam_ValidInput_ReturnsTrue()
        {
            var context = SetTeamDatabase();
            var teamService = new TeamService(context);

            await teamService.CreateTeamAsync("test");
            var team = await context.Teams.FirstOrDefaultAsync(t => t.Name == "test");

            var result = await teamService.EditTeamAsync(team.Id, "test");

            Assert.True(result);

            await context.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task EditTeam_InvalidName_ReturnsFalse()
        {
            var context = SetTeamDatabase();
            var teamService = new TeamService(context);

            await teamService.CreateTeamAsync("test");
            await teamService.CreateTeamAsync("edited");
            var team = await context.Teams.FirstOrDefaultAsync(t => t.Name == "test");

            var result = await teamService.EditTeamAsync(team.Id, "edited");

            Assert.False(result);

            await context.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task DeleteTeam_ValidInput_ReturnsTrue()
        {
            var context = SetTeamDatabase();
            var teamService = new TeamService(context);

            await teamService.CreateTeamAsync("test");
            var team = await context.Teams.FirstOrDefaultAsync(t => t.Name == "test");

            var result = await teamService.DeleteTeamAsync(team.Id);

            Assert.True(result);

            await context.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task DeleteTeam_InvalidInput_ReturnsFalse()
        {
            var context = SetTeamDatabase();
            var userService = new UserService(context);
            var teamService = new TeamService(context);
            var projectService = new ProjectService(context);
            var TaskService = new TaskProjService(context);

            await teamService.CreateTeamAsync("test");
            var team = await context.Teams.FirstOrDefaultAsync(t => t.Name == "test");

            await userService.CreateUserAsync("test", "testpass", "testcho", "testkov", true, team.Id);
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == "test");

            await teamService.AssignUserToTeamAsync(team.Id, user.Id);

            await projectService.CreateProjectAsync("test", user.Id);
            var project = await context.Projects.FirstOrDefaultAsync(p => p.Name == "test");

            await TaskService.CreateTaskAsync("test", user.Id, user.Id, project.Id, false);

            var result = await teamService.DeleteTeamAsync(team.Id);

            Assert.False(result);

            await context.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task AssignUserToTeam_ValidInput_ReturnsTrue()
        {
            var context = SetTeamDatabase();
            var teamService = new TeamService(context);
            var userService = new UserService(context);

            await teamService.CreateTeamAsync("test");
            var team = await context.Teams.FirstOrDefaultAsync(t => t.Name == "test");

            await userService.CreateUserAsync("test", "testpass", "testcho", "testkov", true, 0);
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == "test");

            var result = await teamService.AssignUserToTeamAsync(team.Id, user.Id);

            Assert.True(result);

            await context.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task AssignUserToTeam_InvalidInput_ReturnsFalse()
        {
            var context = SetTeamDatabase();
            var teamService = new TeamService(context);
            var userService = new UserService(context);

            await teamService.CreateTeamAsync("test");
            var team = await context.Teams.FirstOrDefaultAsync(t => t.Name == "test");

            await userService.CreateUserAsync("test", "testpass", "testcho", "testkov", true, 0);
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == "test");

            await teamService.AssignUserToTeamAsync(team.Id, user.Id);
            var result = await teamService.AssignUserToTeamAsync(team.Id, user.Id);

            Assert.False(result);

            await context.Database.EnsureDeletedAsync();
        }

        private DatabaseContext SetTeamDatabase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "teamDb");

            var context = new DatabaseContext(optionsBuilder.Options);

            return context;
        }
    }
}
