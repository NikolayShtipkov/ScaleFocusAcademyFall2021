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
    public class ProjectService : IProjectService
    {
        private readonly DatabaseContext _context;

        public ProjectService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Project>> GetAllProjectsAsync(int id)
        {
            var accessibleProjects = await _context.Projects.Where(p => p.OwnerId == id).ToListAsync();

            foreach (var pt in await _context.ProjectTeam.ToListAsync())
            {
                foreach (var at in await _context.TeamUser.Where(tu => tu.UserId == id).ToListAsync())
                {
                    if (pt.TeamId == at.TeamId)
                    {
                        var project = await GetProjectByIdAsync(pt.ProjectId);
                        if (project.OwnerId != id)
                        {
                            accessibleProjects.Add(project);
                        }
                    }
                }
            }

            return accessibleProjects;
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            return await _context.Projects.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> CreateProjectAsync(string name, int ownerId)
        {
            if (await _context.Projects.AnyAsync(p => p.Name == name))
            {
                throw new AlreadyExistsException("Name is already in use.");
            }

            var project = new Project()
            {
                Name = name,
                OwnerId = ownerId
            };

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditProjectAsync(int projectId, int userId, string name)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
            if (project == null)
            {
                throw new NotFoundException("Invalid input. Project Id doesn't exist.");
            }

            if (project.OwnerId != userId)
            {
                throw new UnauthorizedException("You have to be the owner of the project to make changes to it.");
            }

            if (await _context.Projects.AnyAsync(p => p.Name == name) && project.Name != name)
            {
                throw new AlreadyExistsException("Name is already in use.");
            }

            project.Name = name;

            _context.Projects.Update(project);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteProjectAsync(int projectId, int userId)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
            if (project == null)
            {
                throw new NotFoundException("Invalid input. Project Id doesn't exist.");
            }

            if (project.OwnerId != userId)
            {
                throw new UnauthorizedException("You have to be the owner of the project to delete it.");
            }

            _context.Projects.Remove(project);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AssignTeamToProjectAsync(int projectId, int teamId, int userId)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
            if (project == null)
            {
                throw new NotFoundException("Invalid input. Project Id doesn't exist.");
            }

            if (project.OwnerId != userId)
            {
                throw new ForbiddenAccessException("You have to be the owner of the project to make changes to it.");
            }

            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == teamId);
            if (team == null)
            {
                throw new NotFoundException($"Invalid input. Team Id doesn't exist.");
            }

            if (await _context.ProjectTeam.AnyAsync(pt => pt.ProjectId == projectId && pt.TeamId == teamId))
            {
                throw new AlreadyExistsException("This team is already assigned to the project.");
            }

            var projectTeam = new ProjectTeam()
            {
                ProjectId = projectId,
                TeamId = teamId
            };

            await _context.ProjectTeam.AddAsync(projectTeam);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> HasAccess(int projectId, int userId)
        {
            if (await _context.Projects.AnyAsync(p => p.Id == projectId && p.OwnerId == userId))
            {
                return true;
            }

            foreach (var pt in await _context.ProjectTeam.Where(pt => pt.ProjectId == projectId).ToListAsync())
            {
                foreach (var at in await _context.TeamUser.Where(tu => tu.UserId == userId).ToListAsync())
                {
                    if (pt.TeamId == at.TeamId)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
