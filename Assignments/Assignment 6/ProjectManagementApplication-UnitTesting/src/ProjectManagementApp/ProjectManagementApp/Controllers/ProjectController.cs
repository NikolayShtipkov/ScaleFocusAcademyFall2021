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
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace ProjectManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private static IUserService _userService;
        private static ITeamService _teamService;
        private static IProjectService _projectService;

        public ProjectController(IUserService userService, ITeamService teamService, IProjectService projectService) : base()
        {
            _userService = userService;
            _teamService = teamService;
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectResponseDTO>>> GetAllProjectsAsync()
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            var projects = await _projectService.GetAllProjectsAsync(currentUser.Id);
            var projectResponseDTO = new List<ProjectResponseDTO>();
            foreach (var project in projects)
            {
                projectResponseDTO.Add(MapProject(project));
            }

            return projectResponseDTO;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProjectAsync(ProjectRequestDTO project)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            if (ModelState.IsValid)
            {
                bool isCreated = await _projectService.CreateProjectAsync(project.Name, currentUser.Id);
                if (isCreated)
                {
                    return Ok("Project created successfully.");
                }
            }

            return Conflict("Name is already in use.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditProjectAsync(int id, ProjectRequestDTO projectEdit)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound("Invalid input. Project Id doesn't exist.");
            }

            if (project.OwnerId != currentUser.Id)
            {
                return Unauthorized("You have to be the owner of the project to make changes to it.");
            }

            bool isEdited = await _projectService.EditProjectAsync(id, projectEdit.Name);
            if (!isEdited)
            {
                return Conflict("Name already in use.");
            }

            return Ok($"Project with Id: {id} was edited successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectAsync(int id)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound("Invalid input. Project Id doesn't exist.");
            }

            if (project.OwnerId != currentUser.Id)
            {
                return Unauthorized("You have to be the owner of the project to delete it.");
            }

            bool isDeleted = await _projectService.DeleteProjectAsync(id);
            if (!isDeleted)
            {
                return BadRequest("Project can't be deleted, please check input.");
            }

            return Ok($"Project with Id: {id} was deleted successfully.");
        }

        [HttpPost("{projectId}/Assign/{teamId}")]
        public async Task<IActionResult> AssignTeamToProjectAsync(int projectId, int teamId)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            var project = await _projectService.GetProjectByIdAsync(projectId);
            if (project == null)
            {
                return NotFound($"Invalid input. Project with Id: {projectId} doesn't exist.");
            }

            if (project.OwnerId != currentUser.Id)
            {
                return StatusCode(403);
            }

            var team = await _teamService.GetTeamByIdAsync(teamId);
            if (team == null)
            {
                return NotFound($"Invalid input. Team with Id: {teamId} doesn't exist.");
            }

            if (ModelState.IsValid)
            {
                bool isCreated = await _projectService.AssignTeamToProjectAsync(projectId, teamId);
                if (isCreated)
                {
                    return Ok($"Team with Id: {teamId} has been assigned to project with Id: {projectId} successfully.");
                }
            }

            return Ok($"Team with Id: {teamId} is already a member of project with Id: {projectId}.");
        }

        private ProjectResponseDTO MapProject(Project projectEntity)
        {
            var project = new ProjectResponseDTO()
            {
                Id = projectEntity.Id,
                Name = projectEntity.Name,
                OwnerId = projectEntity.OwnerId
            };

            return project;
        }
    }
}
