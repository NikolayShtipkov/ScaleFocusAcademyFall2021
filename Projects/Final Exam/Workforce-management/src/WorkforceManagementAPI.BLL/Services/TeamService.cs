using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Contracts;
using WorkforceManagementAPI.DAL.Contracts;
using WorkforceManagementAPI.DAL.Contracts.IdentityContracts;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DTO.Models.Requests;

namespace WorkforceManagementAPI.BLL.Services
{
    public class TeamService : ITeamService
    {
        private readonly IValidationService _validationService;
        private readonly ITeamRepository _teamRepository;
        private readonly IMapper _mapper;
        private readonly IIdentityUserManager _userManager;

        public TeamService(IValidationService validationService, ITeamRepository teamRepository, IIdentityUserManager userManager, IMapper mapper)
        {
            _validationService = validationService;
            _teamRepository = teamRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<List<Team>> GetAllTeamsAsync()
        {
            return await _teamRepository.GetAllTeamsAsync();
        }

        public async Task<List<Team>> GetMyTeamsAsync(string userId)
        {
            return await _teamRepository.GetMyTeamsAsync(userId);
        }

        public async Task<Team> GetTeamByIdAsync(Guid teamId)
        {
            var team = await _teamRepository.GetTeamByIdAsync(teamId);
            _validationService.EnsureTeamExist(team);

            return team;
        }

        public async Task<bool> CreateTeamAsync(TeamRequestDTO teamRequest, string creatorId)
        {
            _validationService.EnsureTeamNameIsUnique(teamRequest.Title);

            var now = DateTime.Now;
            var team = _mapper.Map<Team>(teamRequest);
            team.CreatorId = creatorId;
            team.ModifierId = creatorId;
            team.CreatedAt = now;
            team.ModifiedAt = now;

            await _teamRepository.AddTeamAsync(team);
            await _teamRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditTeamAsync(Guid teamId, string modifierId, TeamRequestDTO editTeamRequest)
        {
            var team = await _teamRepository.GetTeamByIdAsync(teamId);
            _validationService.EnsureTeamExist(team);
            _validationService.EnsureTeamNameIsUniqueWhenEdit(editTeamRequest.Title, team.Title);

            team.Title = editTeamRequest.Title;
            team.Description = editTeamRequest.Description;
            team.ModifierId = modifierId;
            team.ModifiedAt = DateTime.Now;

            _teamRepository.UpdateTeam(team);
            await _teamRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteTeamAsync(Guid teamId)
        {
            var team = await _teamRepository.GetTeamByIdAsync(teamId);
            _validationService.EnsureTeamExist(team);

            _teamRepository.RemoveTeam(team);
            await _teamRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AssignUserToTeamAsync(Guid teamId, string userId, string modifierId)
        {
            var team = await _teamRepository.GetTeamByIdAsync(teamId);
            _validationService.EnsureTeamExist(team);

            var user = await _userManager.FindByIdAsync(userId);
            _validationService.EnsureUserExist(user);

            _validationService.EnsureUserIsNotAlreadyPartOfTheTeam(team, user);

            if (team.Users.Count == 0)
            {
                team.TeamLeaderId = userId;
                _teamRepository.UpdateTeam(team);
            }
            team.ModifierId = modifierId;
            team.ModifiedAt = DateTime.Now;
            _teamRepository.UpdateTeam(team);
            _teamRepository.AddTeamUser(team, user);

            await _teamRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UnassignUserFromTeamAsync(Guid teamId, string userId, string modifierId)
        {
            var team = await _teamRepository.GetTeamByIdAsync(teamId);
            _validationService.EnsureTeamExist(team);

            var user = await _userManager.FindByIdAsync(userId);
            _validationService.EnsureUserExist(user);

            _validationService.EnsureUserIsNotAlreadyATeamLeader(team, user);
            _validationService.EnsureUnassignUserHasAccessToTeam(team, user);

            team.ModifierId = modifierId;
            team.ModifiedAt = DateTime.Now;
            _teamRepository.UpdateTeam(team);

            _teamRepository.RemoveTeamUser(team, user);

            await _teamRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AssignTeamLeaderAsync(Guid teamId, string userId ,string modifierId)
        {
            var team = await _teamRepository.GetTeamByIdAsync(teamId);
            _validationService.EnsureTeamExist(team);

            var user = await _userManager.FindByIdAsync(userId);
            _validationService.EnsureUserExist(user);

            _validationService.EnsureUserIsNotAlreadyATeamLeader(team, user);
            _validationService.EnsureUserHasAccessToTeam(team, user);

            team.TeamLeaderId = userId;
            team.ModifiedAt = DateTime.Now;
            team.ModifierId = modifierId;
            _teamRepository.UpdateTeam(team);
            await _teamRepository.SaveChangesAsync();

            return true;
        }
    }
}
