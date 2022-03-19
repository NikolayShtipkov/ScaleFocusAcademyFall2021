using ProjectManagementApp.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementApp.Interfaces
{
    public interface ITaskProjService
    {
        Task<bool> CreateTaskAsync(string name, int ownerId, int assigneeId, int projectId, bool IsCompleted);
        Task<bool> DeleteTaskAsync(int taskId);
        Task<bool> EditTaskAsync(int id, string name, int assigneeId);
        Task<TaskProj> GetTaskByIdAsync(int id);
        Task<List<TaskProj>> GetTasksFromProjectAsync(int id);
        Task<bool> TaskStatusChangeAsync(int taskId, bool isCompleted);
    }
}