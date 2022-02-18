using ProjectManagementApp.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementApp.Interfaces
{
    public interface IWorkLogService
    {
        Task<bool> CreateWorkLogAsync(int timeWorked, int taskId);
        Task<bool> DeleteWorkLogAsync(int worklogId);
        Task<bool> EditWorkLogAsync(int id, int timeWorked);
        Task<WorkLog> GetWorkLogByIdAsync(int id);
        Task<List<WorkLog>> GetWorkLogsFromTasks(int id);
    }
}