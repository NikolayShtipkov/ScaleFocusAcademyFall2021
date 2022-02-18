using Microsoft.EntityFrameworkCore;
using ProjectManagementApp.Data.Database;
using ProjectManagementApp.Data.Entities;
using ProjectManagementApp.Interfaces;
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
                return false;
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

        public async Task<bool> EditProjectAsync(int id, string name)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
            if (await _context.Projects.AnyAsync(p => p.Name == name) && project.Name != name)
            {
                return false;
            }

            project.Name = name;

            _context.Projects.Update(project);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteProjectAsync(int projectId)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

            _context.Projects.Remove(project);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AssignTeamToProjectAsync(int projectId, int teamId)
        {
            if (await _context.ProjectTeam.AnyAsync(pt => pt.ProjectId == projectId && pt.TeamId == teamId))
            {
                return false;
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
