using Microsoft.AspNetCore.Mvc;
using ProjectManagementApp.Auth;
using ProjectManagementApp.DTO.Requests;
using ProjectManagementApp.DTO.Responses;
using ProjectManagementApp.Data.Entities;
using ProjectManagementApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManagementApp.Interfaces;

namespace ProjectManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private static IUserService _userService;
        private static ITeamService _teamService;

        public TeamController(IUserService userService, ITeamService teamService) : base()
        {
            _userService = userService;
            _teamService = teamService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamResponseDTO>>> GetAllTeamsAsync()
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            if (!currentUser.IsAdmin)
            {
                return Unauthorized("You must have administritive privilages to access this service.");
            }

            var teams = await _teamService.GetAllTeamsAsync();
            var teamResponseDTO = new List<TeamResponseDTO>();
            foreach (var team in teams)
            {
                teamResponseDTO.Add(MapTeam(team));
            }

            return teamResponseDTO;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TeamResponseDTO>> GetTeamByIdAsync(int id)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            if (!currentUser.IsAdmin)
            {
                return Unauthorized("You must have administritive privilages to use this service.");
            }

            var team = await _teamService.GetTeamByIdAsync(id);
            if (team == null)
            {
                return NotFound("Invalid input. Team Id doesn't exist.");
            }

            return MapTeam(team);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeamAsync(TeamRequestDTO team)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            if (!currentUser.IsAdmin)
            {
                return Unauthorized("You must have administritive privilages to access this services.");
            }

            if (ModelState.IsValid)
            {
                bool isCreated = await _teamService.CreateTeamAsync(team.Name);
                if (isCreated)
                {
                    return Ok("Team created successfully.");
                }
            }

            return Conflict("Name is already in use.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditTeamAsync(int id, TeamRequestDTO teamEdit)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            if (!currentUser.IsAdmin)
            {
                return Unauthorized("You must have administritive privilages to use this service.");
            }

            var team = await _teamService.GetTeamByIdAsync(id);
            if (team == null)
            {
                return NotFound("Invalid input. User Id doesn't exist.");
            }

            bool isEdited = await _teamService.EditTeamAsync(id, teamEdit.Name);
            if (!isEdited)
            {
                return Conflict("Name already in use.");
            }

            return Ok($"Team with Id: {id} was edited successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeamAsync(int id)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            if (!currentUser.IsAdmin)
            {
                return Unauthorized("You must have administritive privilages to use this service.");
            }

            var team = await _teamService.GetTeamByIdAsync(id);
            if (team == null)
            {
                return NotFound("Invalid input. Team Id doesn't exist.");
            }

            bool isDeleted = await _teamService.DeleteTeamAsync(id);
            if (!isDeleted)
            {
                return BadRequest("This team has a member with a task assigned to them and can't be deleted.");
            }

            return Ok($"Team with Id: {id} was deleted successfully");
        }

        [HttpPost("{teamId}/Assign/{userId}")]
        public async Task<IActionResult> AssignUserToTeamAsync(int teamId, int userId)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            if (!currentUser.IsAdmin)
            {
                return Unauthorized("You must have administritive privilages to access this services.");
            }

            var team = await _teamService.GetTeamByIdAsync(teamId);
            if (team == null)
            {
                return NotFound($"Invalid input. Team with Id: {teamId} doesn't exist.");
            }

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Invalid input. User with Id: {userId} doesn't exist.");
            }

            if (ModelState.IsValid)
            {
                bool isCreated = await _teamService.AssignUserToTeamAsync(teamId, userId);
                if (isCreated)
                {
                    return Ok($"User with Id: {userId} has been assigned to team with Id: {teamId} successfully.");
                }
            }

            return Ok($"User with Id: {userId} is already a member of team with Id: {teamId}.");
        }

        [HttpGet("/Assigned")]
        public async Task<ActionResult<IEnumerable<TeamUserResponseDTO>>> GetTeamUsersAsync()
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            if (!currentUser.IsAdmin)
            {
                return Unauthorized("You must have administritive privilages to access user services.");
            }

            var teamUsers = await _teamService.GetTeamUsersAsync();
            var teamUserResponseDTO = new List<TeamUserResponseDTO>();
            foreach (var tu in teamUsers)
            {
                teamUserResponseDTO.Add( new TeamUserResponseDTO() 
                { 
                    TeamId = tu.TeamId,
                    UserId = tu.UserId
                });
            }

            return teamUserResponseDTO;
        }

        private TeamResponseDTO MapTeam(Team teamEntity)
        {
            var team = new TeamResponseDTO()
            {
                Id = teamEntity.Id,
                Name = teamEntity.Name
            };

            return team;
        }
    }
}
