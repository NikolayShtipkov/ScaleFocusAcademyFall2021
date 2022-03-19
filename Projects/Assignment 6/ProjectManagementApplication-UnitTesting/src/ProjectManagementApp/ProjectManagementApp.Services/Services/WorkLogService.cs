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
    public class WorkLogService : IWorkLogService
    {
        private readonly DatabaseContext _context;

        public WorkLogService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<WorkLog>> GetWorkLogsFromTasks(int id)
        {
            return await _context.WorkLogs.Where(w => w.TaskId == id).ToListAsync();
        }

        public async Task<WorkLog> GetWorkLogByIdAsync(int id)
        {
            return await _context.WorkLogs.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> CreateWorkLogAsync(int timeWorked, int taskId)
        {
            DateTime date = DateTime.Now;

            foreach (var w in await _context.WorkLogs.Where(w => w.TaskId == taskId).ToListAsync())
            {
                if (w.CreatedAt.Date == date.Date)
                {
                    return false;
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

        public async Task<bool> EditWorkLogAsync(int id, int timeWorked)
        {
            if (timeWorked > 24)
            {
                return false;
            }

            var workLog = await _context.WorkLogs.FirstOrDefaultAsync(t => t.Id == id);

            workLog.TimeWorked = timeWorked;

            _context.WorkLogs.Update(workLog);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteWorkLogAsync(int worklogId)
        {
            var worklog = await _context.WorkLogs.FirstOrDefaultAsync(w => w.Id == worklogId);

            _context.WorkLogs.Remove(worklog);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
