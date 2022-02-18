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
    public class TaskProjService : ITaskProjService
    {
        private readonly DatabaseContext _context;

        public TaskProjService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<TaskProj>> GetTasksFromProjectAsync(int id)
        {
            return await _context.Tasks.Where(t => t.ProjectId == id).ToListAsync();
        }

        public async Task<TaskProj> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> CreateTaskAsync(string name, int ownerId, int assigneeId, int projectId, bool IsCompleted)
        {
            if (await _context.Tasks.AnyAsync(t => t.Name == name))
            {
                return false;
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

        public async Task<bool> EditTaskAsync(int id, string name, int assigneeId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (await _context.Tasks.AnyAsync(p => p.Name == name) && task.Name != name)
            {
                return false;
            }

            if (assigneeId > 0)
            {
                task.AssigneeId = assigneeId;
            }

            task.Name = name;

            _context.Tasks.Update(task);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteTaskAsync(int taskId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);

            _context.Tasks.Remove(task);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> TaskStatusChangeAsync(int taskId, bool isCompleted)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);

            task.IsCompleted = isCompleted;

            _context.Tasks.Update(task);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
