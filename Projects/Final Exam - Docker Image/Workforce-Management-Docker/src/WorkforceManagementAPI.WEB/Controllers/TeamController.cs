using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Contracts;
using WorkforceManagementAPI.BLL.Services;
using WorkforceManagementAPI.Common;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DTO.Models.Requests;
using WorkforceManagementAPI.DTO.Models.Responses;

namespace WorkforceManagementAPI.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class TeamController : ControllerBase
    {
        private static IUserService _userService;
        private static ITeamService _teamService;
        private readonly IMapper _mapper;
        private User currentUser;
        
        public TeamController(IUserService userService, ITeamService teamService, IMapper mapper) : base()
        {
            _userService = userService;
            _teamService = teamService;
            _mapper = mapper;
        }

        /// <summary>
        /// List all teams, existing in the database.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">OK - Request succeeded.</response>
        /// <response code="401">Unauthorized - Please check the provided credentials.</response>
        /// <response code="403">Forbidden - Your credentials don't meet the required authorization level to access the resource. 
        ///Please, contact your administrator to get desired permissions.</response>
        [HttpGet]
        public async Task<IEnumerable<TeamResponseDTO>> GetAllTeamsAsync()
        {
            var teams = await _teamService.GetAllTeamsAsync();
            return _mapper.Map<IEnumerable<TeamResponseDTO>>(teams);
        }

        /// <summary>
        /// Find a team by team Id.
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        /// <response code="200">OK - Request succeeded.</response>
        /// <response code="400">BadRequest - Request could not be understood by the server.</response>
        /// <response code="401">Unauthorized - Please check the provided credentials.</response>
        /// <response code="403">Forbidden - Your credentials don't meet the required authorization level to access the resource. 
        ///Please, contact your administrator to get desired permissions.</response>
        /// <response code="404">NotFound - Requested information does not exist in the server.</response>
        /// <response code="500">InternalServerError - Generic error occured in the server.</response>
        [Authorize]
        [HttpGet("{teamId}")]
        public async Task<TeamResponseDTO> GetTeamByIdAsync(Guid teamId)
        {
            var team = await _teamService.GetTeamByIdAsync(teamId);

            return _mapper.Map<TeamResponseDTO>(team);
        }

        /// <summary>
        /// List all teams, the logged user is member of.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">OK - Request succeeded.</response>
        /// <response code="401">Unauthorized - Please check the provided credentials.</response>
        /// <response code="403">Forbidden - Your credentials don't meet the required authorization level to access the resource. 
        ///Please, contact your administrator to get desired permissions.</response>
        [Authorize]
        [HttpGet("MyTeams")]
        public async Task<IEnumerable<TeamResponseDTO>> GetMyTeamsAsync()
        {
            currentUser = await _userService.GetCurrentUser(User);

            var teams = await _teamService.GetMyTeamsAsync(currentUser.Id);
            return _mapper.Map<IEnumerable<TeamResponseDTO>>(teams);
        }

        /// <summary>
        /// Create a team.
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        /// <response code="201">Created - Request resulted in new resource created.</response>
        /// <response code="400">BadRequest - Request could not be understood by the server.</response>
        /// <response code="401">Unauthorized - Please check the provided credentials.</response>
        /// <response code="403">Forbidden - Your credentials don't meet the required authorization level to access the resource. 
        ///Please, contact your administrator to get desired permissions.</response>
        /// <response code="404">NotFound - Requested information does not exist in the server.</response>
        /// <response code="409">Conflict - The submitted entity ran into a conflict with an existing one.</response>
        /// <response code="500">InternalServerError - Generic error occured in the server.</response>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> CreateTeamAsync(TeamRequestDTO team)
        {
            currentUser = await _userService.GetCurrentUser(User);

            bool isCreated = await _teamService.CreateTeamAsync(team, currentUser.Id);
            if (isCreated && ModelState.IsValid)
            {
                return Created(nameof(HttpPostAttribute), String.Format(Constants.Created, "Team"));
            }

            return BadRequest();
        }

        /// <summary>
        /// Edit a team by team Id.
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="teamEdit"></param>
        /// <returns></returns>
        /// <response code="200">OK - Request succeeded.</response>
        /// <response code="400">BadRequest - Request could not be understood by the server.</response>
        /// <response code="401">Unauthorized - Please check the provided credentials.</response>
        /// <response code="403">Forbidden - Your credentials don't meet the required authorization level to access the resource. 
        ///Please, contact your administrator to get desired permissions.</response>
        /// <response code="404">NotFound - Requested information does not exist in the server.</response>
        /// <response code="409">Conflict - The submitted entity ran into a conflict with an existing one.</response>
        /// <response code="500">InternalServerError - Generic error occured in the server.</response>
        [Authorize(Roles = "Admin")]
        [HttpPut("{teamId}")]
        public async Task<IActionResult> EditTeamAsync(Guid teamId, TeamRequestDTO teamEdit)
        {
            currentUser = await _userService.GetCurrentUser(User);

            bool isEdited = await _teamService.EditTeamAsync(teamId, currentUser.Id, teamEdit);
            if (isEdited)
            {
                return Ok("Team edited successfully.");
            }

            return BadRequest();
        }

        /// <summary>
        /// REMOVE a team by team Id.
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        /// <response code="200">OK - Request succeeded.</response>
        /// <response code="400">BadRequest - Request could not be understood by the server.</response>
        /// <response code="401">Unauthorized - Please check the provided credentials.</response>
        /// <response code="403">Forbidden - Your credentials don't meet the required authorization level to access the resource. 
        ///Please, contact your administrator to get desired permissions.</response>
        /// <response code="404">NotFound - Requested information does not exist in the server.</response>
        /// <response code="409">Conflict - The submitted entity ran into a conflict with an existing one.</response>
        /// <response code="500">InternalServerError - Generic error occured in the server.</response>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{teamId}")]
        public async Task<IActionResult> DeleteTeamAsync(Guid teamId)
        {
            currentUser = await _userService.GetCurrentUser(User);

            bool isDeleted = await _teamService.DeleteTeamAsync(teamId);
            if (isDeleted)
            {
                return Ok("Team deleted successfully.");
            }

            return BadRequest();
        }

        /// <summary>
        /// Assign user to a team, using user Id and team Id.
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <response code="200">OK - Request succeeded.</response>
        /// <response code="400">BadRequest - Request could not be understood by the server.</response>
        /// <response code="401">Unauthorized - Please check the provided credentials.</response>
        /// <response code="403">Forbidden - Your credentials don't meet the required authorization level to access the resource. 
        ///Please, contact your administrator to get desired permissions.</response>
        /// <response code="404">NotFound - Requested information does not exist in the server.</response>
        /// <response code="409">Conflict - The submitted entity ran into a conflict with an existing one.</response>
        /// <response code="500">InternalServerError - Generic error occured in the server.</response>
        [Authorize(Roles = "Admin")]
        [HttpPost("{teamId}/Assign/{userId}")]
        public async Task<IActionResult> AssignUserToTeamAsync(Guid teamId, string userId)
        {
            currentUser = await _userService.GetCurrentUser(User);

            bool isAssigned = await _teamService.AssignUserToTeamAsync(teamId, userId, currentUser.Id);
            if (isAssigned)
            {
                return Ok("User assigned to team successfully.");
            }

            return BadRequest();
        }

        /// <summary>
        /// REMOVE user from a team, using user Id and team Id.
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <response code="200">OK - Request succeeded.</response>
        /// <response code="400">BadRequest - Request could not be understood by the server.</response>
        /// <response code="401">Unauthorized - Please check the provided credentials.</response>
        /// <response code="403">Forbidden - Your credentials don't meet the required authorization level to access the resource. 
        ///Please, contact your administrator to get desired permissions.</response>
        /// <response code="404">NotFound - Requested information does not exist in the server.</response>
        /// <response code="409">Conflict - The submitted entity ran into a conflict with an existing one.</response>
        /// <response code="500">InternalServerError - Generic error occured in the server.</response>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{teamId}/Unassign/{userId}")]
        public async Task<IActionResult> UnassignUserFromTeamAsync(Guid teamId, string userId)
        {
            currentUser = await _userService.GetCurrentUser(User);

            bool isUnassigned = await _teamService.UnassignUserFromTeamAsync(teamId, userId, currentUser.Id);
            if (isUnassigned)
            {
                return Ok("User unassigned from team successfully.");
            }

            return BadRequest();
        }

        /// <summary>
        /// ASSIGN user as a TEAM LEADER, using user Id and team Id.
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <response code="200">OK - Request succeeded.</response>
        /// <response code="400">BadRequest - Request could not be understood by the server.</response>
        /// <response code="401">Unauthorized - Please check the provided credentials.</response>
        /// <response code="403">Forbidden - Your credentials don't meet the required authorization level to access the resource. 
        ///Please, contact your administrator to get desired permissions.</response>
        /// <response code="404">NotFound - Requested information does not exist in the server.</response>
        /// <response code="409">Conflict - The submitted entity ran into a conflict with an existing one.</response>
        /// <response code="500">InternalServerError - Generic error occured in the server.</response>
        [Authorize(Roles = "Admin")]
        [HttpPut("{teamId}/AssignLeader/{userId}")]
        public async Task<IActionResult> AssignTeamLeaderAsync(Guid teamId, string userId)
        {
            currentUser = await _userService.GetCurrentUser(User);

            bool isAssigned = await _teamService.AssignTeamLeaderAsync(teamId, userId , currentUser.Id);
            if (isAssigned)
            {
                return Ok("Team leader assigned to team successfully.");
            }

            return BadRequest();
        }

    }
}
