using Microsoft.AspNetCore.Mvc;
using ProjectManagementApp.Auth;
using ProjectManagementApp.Data.DTO.Requests;
using ProjectManagementApp.Data.DTO.Responses;
using ProjectManagementApp.Data.Entities;
using ProjectManagementApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskProjController : ControllerBase
    {
        private static UserService _userService;
        private static ProjectService _projectService;
        private static TaskProjService _taskProjService;

        public TaskProjController(UserService userService, ProjectService projectService, TaskProjService taskProjService) : base()
        {
            _userService = userService;
            _projectService = projectService;
            _taskProjService = taskProjService;
        }

        [HttpGet("{projectId}")]
        public async Task<ActionResult<IEnumerable<TaskProjResponseDTO>>> GetTasksFromProjectAsync(int projectId)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            var project = await _projectService.GetProjectByIdAsync(projectId);
            if (project == null)
            {
                return NotFound("Invalid input, project Id doesn't exist.");
            }

            if (!await _projectService.HasAccess(projectId, currentUser.Id))
            {
                return Unauthorized("User must be owner or assigned to a team thats part of the project to view tasks.");
            }

            var tasks = await _taskProjService.GetTasksFromProjectAsync(projectId);
            if (tasks.Count == 0)
            {
                return Ok("No tasks created in this project.");
            }

            var taskResponseDTO = new List<TaskProjResponseDTO>();
            foreach (var task in tasks)
            {
                taskResponseDTO.Add(MapTask(task));
            }

            return taskResponseDTO;
        }

        [HttpPost]
        public async Task<ActionResult> CreateTaskAsync(TaskProjCreateRequestDTO task)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            var asignee = await _userService.GetUserByIdAsync(task.AssigneeId);
            if (task.AssigneeId == 0 || asignee == null)
            {
                return NotFound("Invalid input, Assignee Id doesn't exist.");
            }

            var project = await _projectService.GetProjectByIdAsync(task.ProjectId);
            if (task.ProjectId == 0 || project == null)
            {
                return NotFound("Invalid input, project Id doesn't exist.");
            }

            if (project.OwnerId != currentUser.Id)
            {
                return Unauthorized("You have to be the owner of a project to create tasks in it.");
            }

            if (!await _projectService.HasAccess(project.Id, asignee.Id))
            {
                return Unauthorized("User must be in a team that is part of the project or the owner himself to be assigned to a task.");
            }

            if (ModelState.IsValid)
            {
                bool isCreated = await _taskProjService.CreateTaskAsync(task.Name, currentUser.Id, task.AssigneeId, task.ProjectId, task.IsCompleted);
                if (isCreated)
                {
                    return Ok("Task created successfully.");
                }
            }

            return BadRequest("Name is already in use.");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditTaskAsync(int id, TaskProjEditRequestDTO taskEdit)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            var task = await _taskProjService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound("Invalid input. Task Id doesn't exist.");
            }

            if (task.OwnerId != currentUser.Id)
            {
                return Unauthorized("You have to be the owner of the task to make changes to it.");
            }

            bool isEdited = await _taskProjService.EditTaskAsync(id, taskEdit.Name, taskEdit.AssigneeId);
            if (!isEdited)
            {
                return BadRequest("Name already in use.");
            }

            return Ok($"Task with Id: {id} was edited successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTaskAsync(int id)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            var task = await _taskProjService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound("Invalid input. Task Id doesn't exist.");
            }

            if (task.OwnerId != currentUser.Id)
            {
                return Unauthorized("You have to be the owner of the task to delete it.");
            }

            bool isDeleted = await _taskProjService.DeleteTaskAsync(id);
            if (!isDeleted)
            {
                return BadRequest("Task can't be deleted, please check input.");
            }

            return Ok($"Task with Id: {id} was deleted successfully.");
        }

        [HttpPut("Status/{id}")]
        public async Task<ActionResult> TaskStatusChangeAsync(int id, TaskProjStatusRequestDTO taskEdit)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            var task = await _taskProjService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound("Invalid input. Task Id doesn't exist.");
            }

            if (task.OwnerId != currentUser.Id && task.AssigneeId == currentUser.Id)
            {
                return Unauthorized("You have to be the owner of the task or be assined to it to change statuses.");
            }

            bool isEdited = await _taskProjService.TaskStatusChangeAsync(id, taskEdit.IsCompleted);
            if (!isEdited)
            {
                return BadRequest("Name already in use.");
            }

            string status = "Pending";
            if (taskEdit.IsCompleted)
            {
                status = "Completed";
            }

            return Ok($"Task status: {status}.");
        }

        private TaskProjResponseDTO MapTask(TaskProj taskEntity)
        {
            string status = "Pending";

            if (taskEntity.IsCompleted)
            {
                status = "Completed";
            }

            var task = new TaskProjResponseDTO()
            {
                Id = taskEntity.Id,
                Name = taskEntity.Name,
                OwnerId = taskEntity.OwnerId,
                AssigneeId = taskEntity.AssigneeId,
                ProjectId = taskEntity.ProjectId,
                Status = status
            };

            return task;
        }
    }
}
