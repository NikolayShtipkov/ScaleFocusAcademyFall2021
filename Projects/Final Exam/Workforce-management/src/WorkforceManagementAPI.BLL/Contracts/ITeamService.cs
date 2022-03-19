using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DTO.Models.Requests;

namespace WorkforceManagementAPI.BLL.Contracts
{
    public interface ITeamService
    {
        Task<bool> AssignTeamLeaderAsync(Guid teamId, string userId, string modifierId);

        Task<bool> AssignUserToTeamAsync(Guid teamId, string userId,string modifierId);

        Task<bool> CreateTeamAsync(TeamRequestDTO teamRequest, string creatorId);

        Task<bool> DeleteTeamAsync(Guid teamId);

        Task<bool> EditTeamAsync(Guid teamId, string modifierId, TeamRequestDTO editTeamRequest);

        Task<List<Team>> GetAllTeamsAsync();

        Task<List<Team>> GetMyTeamsAsync(string userId);

        Task<Team> GetTeamByIdAsync(Guid teamId);

        Task<bool> UnassignUserFromTeamAsync(Guid teamId, string userId ,string modifierId);
    }
}