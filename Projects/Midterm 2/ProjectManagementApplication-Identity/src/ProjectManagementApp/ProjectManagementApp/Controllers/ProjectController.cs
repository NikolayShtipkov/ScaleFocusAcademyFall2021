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
using ProjectManagementApp.Service.Exceptions;

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

            bool isCreated = await _projectService.CreateProjectAsync(project.Name, currentUser.Id);
            if (isCreated && ModelState.IsValid)
            {
                return Ok("Project created successfully.");
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditProjectAsync(int id, ProjectRequestDTO projectEdit)
        {
            var currentUser = _userService.GetCurrentUser(Request);

            bool isEdited = await _projectService.EditProjectAsync(id, currentUser.Id, projectEdit.Name);
            if (isEdited)
            {
                return Ok($"Project with Id: {id} was edited successfully.");
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectAsync(int id)
        {
            var currentUser = _userService.GetCurrentUser(Request);

            bool isDeleted = await _projectService.DeleteProjectAsync(id, currentUser.Id);
            if (isDeleted)
            {
                return Ok($"Project with Id: {id} was deleted successfully.");
            }

            return BadRequest();
        }

        [HttpPost("{projectId}/Assign/{teamId}")]
        public async Task<IActionResult> AssignTeamToProjectAsync(int projectId, int teamId)
        {
            var currentUser = _userService.GetCurrentUser(Request);

            bool isCreated = await _projectService.AssignTeamToProjectAsync(projectId, teamId, currentUser.Id);
            if (isCreated && ModelState.IsValid)
            {
                return Ok($"Team with Id: {teamId} has been assigned to project with Id: {projectId} successfully.");
            }

            return BadRequest();
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
