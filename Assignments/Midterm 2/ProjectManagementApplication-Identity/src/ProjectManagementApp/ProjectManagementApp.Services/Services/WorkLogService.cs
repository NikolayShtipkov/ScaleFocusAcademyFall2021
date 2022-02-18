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
    public class WorkLogService : IWorkLogService
    {
        private readonly DatabaseContext _context;
        private readonly ITaskProjService _taskService;

        public WorkLogService(DatabaseContext context, ITaskProjService taskService)
        {
            _context = context;
            _taskService = taskService;
        }

        public async Task<List<WorkLog>> GetWorkLogsFromTasks(int taskId, int userId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
            if (task == null)
            {
                throw new NotFoundException("Invalid input. Task Id doesn't exist.");
            }

            if (task.AssigneeId != userId && task.OwnerId != userId)
            {
                throw new ForbiddenAccessException("You have to be the owner or be assigned to task to access this service.");
            }

            return await _context.WorkLogs.Where(w => w.TaskId == taskId).ToListAsync();
        }

        public async Task<WorkLog> GetWorkLogByIdAsync(int id)
        {
            return await _context.WorkLogs.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> CreateWorkLogAsync(int timeWorked, int taskId, int userId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
            if (task == null)
            {
                throw new NotFoundException("Invalid input, Task Id doesn't exist.");
            }

            if (task.AssigneeId != userId && task.OwnerId != userId)
            {
                throw new ForbiddenAccessException("You have to be the owner or be assigned to task to access this service.");
            }

            DateTime date = DateTime.Now;
            foreach (var w in await _context.WorkLogs.Where(w => w.TaskId == taskId).ToListAsync())
            {
                if (w.CreatedAt.Date == date.Date)
                {
                    throw new ExceedingLimitException("Can't work more than 24 hours a day.");
                }
            }

            var workLog = new WorkLog()
            {
                CreatedAt = date,
                TimeWorked = timeWorked,
                TaskId = taskId
            };

            await _context.WorkLogs.AddAsync(workLog);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditWorkLogAsync(int worklogId, int userId, int timeWorked)
        {
            var worklog = await _context.WorkLogs.FirstOrDefaultAsync(t => t.Id == worklogId);
            if (worklog == null)
            {
                throw new NotFoundException("Invalid input, Worklog Id doesn't exist.");
            }

            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == worklog.TaskId);
            if (task == null)
            {
                throw new NotFoundException("Invalid input, Task Id doesn't exist.");
            }

            if (task.AssigneeId != userId && task.OwnerId != userId)
            {
                throw new ForbiddenAccessException("You have to be the owner or be assigned to task to access this service.");
            }

            if (timeWorked > 24)
            {
                throw new ExceedingLimitException("Can't work more than 24 hours a day.");
            }

            worklog.TimeWorked = timeWorked;

            _context.WorkLogs.Update(worklog);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteWorkLogAsync(int worklogId, int userId)
        {
            var worklog = await _context.WorkLogs.FirstOrDefaultAsync(t => t.Id == worklogId);
            if (worklog == null)
            {
                throw new NotFoundException("Invalid input, Worklog Id doesn't exist.");
            }

            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == worklog.TaskId);
            if (task == null)
            {
                throw new NotFoundException("Invalid input, Task Id doesn't exist.");
            }

            if (task.AssigneeId != userId && task.OwnerId != userId)
            {
                throw new ForbiddenAccessException("You have to be the owner or be assigned to task to access this service.");
            }

            _context.WorkLogs.Remove(worklog);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
