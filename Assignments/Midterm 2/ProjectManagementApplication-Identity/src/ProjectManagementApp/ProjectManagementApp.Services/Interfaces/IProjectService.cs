using ProjectManagementApp.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementApp.Interfaces
{
    public interface IProjectService
    {
        Task<bool> AssignTeamToProjectAsync(int projectId, int teamId, int userId);
        Task<bool> CreateProjectAsync(string name, int ownerId);
        Task<bool> DeleteProjectAsync(int projectId, int userId);
        Task<bool> EditProjectAsync(int projectId, int userId, string name);
        Task<List<Project>> GetAllProjectsAsync(int id);
        Task<Project> GetProjectByIdAsync(int id);
        Task<bool> HasAccess(int projectId, int userId);
    }
}