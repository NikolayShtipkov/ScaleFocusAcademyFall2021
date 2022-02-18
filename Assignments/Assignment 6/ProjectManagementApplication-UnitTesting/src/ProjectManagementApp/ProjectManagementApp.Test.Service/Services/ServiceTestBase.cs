using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Data.Database;
using ProjectManagementApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.Test.Service.Services
{
    public class ServiceTestBase
    {
        private static DbContextOptionsBuilder<DatabaseContext> _optionsBuilder;
        protected static DatabaseContext _context;

        protected static UserService _userService;
        protected static TeamService _teamService;
        protected static ProjectService _projectService;

        public ServiceTestBase()
        {
            _optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase");
            _context = new DatabaseContext(_optionsBuilder.Options);

            _userService = new UserService(_context);
            _teamService = new TeamService(_context);
            _projectService = new ProjectService(_context);
        }
    }
}
