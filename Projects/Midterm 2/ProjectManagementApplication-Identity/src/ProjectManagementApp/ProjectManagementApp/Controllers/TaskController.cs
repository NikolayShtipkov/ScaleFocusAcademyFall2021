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
    public class TaskController : ControllerBase
    {
        private static IUserService _userService;
        private static IProjectService _projectService;
        private static ITaskProjService _taskProjService;

        public TaskController(IUserService userService, IProjectService projectService, ITaskProjService taskProjService) : base()
        {
            _userService = userService;
            _projectService = projectService;
            _taskProjService = taskProjService;
        }

        [HttpGet("{projectId}")]
        public async Task<ActionResult<IEnumerable<TaskProjResponseDTO>>> GetTasksFromProjectAsync(int projectId)
        {
            var currentUser = _userService.GetCurrentUser(Request);

            var tasks = await _taskProjService.GetTasksFromProjectAsync(projectId, currentUser.Id);

            var taskResponseDTO = new List<TaskProjResponseDTO>();
            foreach (var task in tasks)
            {
                taskResponseDTO.Add(MapTask(task));
            }

            return taskResponseDTO;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTaskAsync(TaskProjCreateRequestDTO task)
        {
            var currentUser = _userService.GetCurrentUser(Request);

            bool isCreated = await _taskProjService.CreateTaskAsync(task.Name, currentUser.Id, task.AssigneeId, task.ProjectId, task.IsCompleted);
            if (isCreated && ModelState.IsValid)
            {
                return Ok("Task created successfully.");
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditTaskAsync(int id, TaskProjEditRequestDTO taskEdit)
        {
            var currentUser = _userService.GetCurrentUser(Request);

            bool isEdited = await _taskProjService.EditTaskAsync(id, taskEdit.Name, taskEdit.AssigneeId, currentUser.Id);
            if (isEdited)
            {
                return Ok($"Task with Id: {id} was edited successfully.");
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTaskAsync(int id)
        {
            var currentUser = _userService.GetCurrentUser(Request);

            bool isDeleted = await _taskProjService.DeleteTaskAsync(id, currentUser.Id);
            if (isDeleted)
            {
                return Ok($"Task with Id: {id} was deleted successfully.");
            }

            return BadRequest();
        }

        [HttpPut("Status/{id}")]
        public async Task<IActionResult> TaskStatusChangeAsync(int id, TaskProjStatusRequestDTO taskEdit)
        {
            var currentUser = _userService.GetCurrentUser(Request);

            bool isEdited = await _taskProjService.TaskStatusChangeAsync(id, currentUser.Id, taskEdit.IsCompleted);
            if (isEdited)
            {
                string status = "Pending";
                if (taskEdit.IsCompleted)
                {
                    status = "Completed";
                }

                return Ok($"Task status: {status}.");
            }

            return BadRequest();
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
