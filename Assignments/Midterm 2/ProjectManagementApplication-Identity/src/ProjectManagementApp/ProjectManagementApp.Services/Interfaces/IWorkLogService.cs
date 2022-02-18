using ProjectManagementApp.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementApp.Interfaces
{
    public interface IWorkLogService
    {
        Task<bool> CreateWorkLogAsync(int timeWorked, int taskId, int userId);
        Task<bool> DeleteWorkLogAsync(int worklogId, int userId);
        Task<bool> EditWorkLogAsync(int worklogId, int userId, int timeWorked);
        Task<WorkLog> GetWorkLogByIdAsync(int id);
        Task<List<WorkLog>> GetWorkLogsFromTasks(int taskId, int userId);
    }
}