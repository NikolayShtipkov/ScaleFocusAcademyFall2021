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
    public class TaskService : ITaskProjService
    {
        private readonly DatabaseContext _context;
        private readonly IProjectService _projectService;

        public TaskService(DatabaseContext context, IProjectService projectService)
        {
            _context = context;
            _projectService = projectService;
        }

        public async Task<List<TaskProj>> GetTasksFromProjectAsync(int projectId, int userId)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
            if (project == null)
            {
                throw new NotFoundException("Invalid input. Project Id doesn't exist.");
            }

            if (!await _projectService.HasAccess(projectId, userId))
            {
                throw new ForbiddenAccessException("You have to be the owner or be assigned to project to access this service.");
            }

            return await _context.Tasks.Where(t => t.ProjectId == projectId).ToListAsync();
        }

        public async Task<TaskProj> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> CreateTaskAsync(string name, int ownerId, int assigneeId, int projectId, bool IsCompleted)
        {
            var assignee = await _context.Users.FirstOrDefaultAsync(u => u.Id == assigneeId);
            if (assigneeId == 0 || assignee == null)
            {
                throw new NotFoundException("Invalid input, Assignee Id doesn't exist.");
            }

            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
            if (projectId == 0 || project == null)
            {
                throw new NotFoundException("Invalid input, Project Id doesn't exist.");
            }

            if (project.OwnerId != ownerId)
            {
                throw new ForbiddenAccessException("You have to be the owner of the project to access this service.");
            }

            if (!await _projectService.HasAccess(projectId, assigneeId))
            {
                throw new ForbiddenAccessException("You have to be the owner or be assigned to project to access this service.");
            }

            if (await _context.Tasks.AnyAsync(t => t.Name == name))
            {
                throw new AlreadyExistsException("Name is already in use.");
            }

            var task = new TaskProj()
            {
                Name = name,
                OwnerId = ownerId,
                AssigneeId = assigneeId,
                ProjectId = projectId,
                IsCompleted = IsCompleted
            };

            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditTaskAsync(int id, string name, int assigneeId, int userId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (task == null)
            {
                throw new NotFoundException("Invalid input, Task Id doesn't exist.");
            }

            if (task.OwnerId != userId)
            {
                throw new ForbiddenAccessException("You have to be the owner of the project to access this service.");
            }

            if (await _context.Tasks.AnyAsync(p => p.Name == name) && task.Name != name)
            {
                throw new AlreadyExistsException("Name is already in use.");
            }

            var assignee = await _context.Users.FirstOrDefaultAsync(u => u.Id == assigneeId);
            if (assigneeId > 0 && assignee != null)
            {
                task.AssigneeId = assigneeId;
            }

            task.Name = name;

            _context.Tasks.Update(task);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteTaskAsync(int taskId, int userId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
            if (task == null)
            {
                throw new NotFoundException("Invalid input, Task Id doesn't exist.");
            }

            if (task.OwnerId != userId)
            {
                throw new ForbiddenAccessException("You have to be the owner of the project to access this service.");
            }

            _context.Tasks.Remove(task);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> TaskStatusChangeAsync(int taskId, int userId, bool isCompleted)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
            if (task == null)
            {
                throw new NotFoundException("Invalid input, Task Id doesn't exist.");
            }

            if (task.OwnerId != userId && task.AssigneeId != userId)
            {
                throw new ForbiddenAccessException("You have to be the owner or be assigned to project to access this service.");
            }

            task.IsCompleted = isCompleted;

            _context.Tasks.Update(task);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
