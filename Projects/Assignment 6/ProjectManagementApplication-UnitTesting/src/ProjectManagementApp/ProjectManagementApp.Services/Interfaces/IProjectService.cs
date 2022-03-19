using ProjectManagementApp.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementApp.Interfaces
{
    public interface IProjectService
    {
        Task<bool> AssignTeamToProjectAsync(int projectId, int teamId);
        Task<bool> CreateProjectAsync(string name, int ownerId);
        Task<bool> DeleteProjectAsync(int projectId);
        Task<bool> EditProjectAsync(int id, string name);
        Task<List<Project>> GetAllProjectsAsync(int id);
        Task<Project> GetProjectByIdAsync(int id);
        Task<bool> HasAccess(int projectId, int userId);
    }
}