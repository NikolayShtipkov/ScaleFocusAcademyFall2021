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
    public class WorkLogController : ControllerBase
    {
        private static IUserService _userService;
        private static ITaskProjService _taskProjService;
        private static IWorkLogService _workLogService;

        public WorkLogController(IUserService userService, IProjectService projectService, ITaskProjService taskProjService, IWorkLogService workLogService) : base()
        {
            _userService = userService;
            _taskProjService = taskProjService;
            _workLogService = workLogService;
        }

        [HttpGet("{taskId}")]
        public async Task<ActionResult<IEnumerable<WorkLogResponseDTO>>> GetTasksFromProjectAsync(int taskId)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            var task = await _taskProjService.GetTaskByIdAsync(taskId);
            if (task == null)
            {
                return NotFound("Invalid input, task Id doesn't exist.");
            }

            if (task.AssigneeId != currentUser.Id && task.OwnerId != currentUser.Id)
            {
                return StatusCode(403);
            }

            var worklogs = await _workLogService.GetWorkLogsFromTasks(taskId);
            var workLogResponses = new List<WorkLogResponseDTO>();
            foreach (var worklog in worklogs)
            {
                string date = worklog.CreatedAt.ToString("dd-MM-yyyy");

                workLogResponses.Add(new WorkLogResponseDTO() 
                {
                    Id = worklog.Id,
                    CreateAt = date,
                    TimeWorked = worklog.TimeWorked,
                    AssigneeId = task.AssigneeId
                });
            }

            return workLogResponses;
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkLogAsync(WorkLogCreateRequestDTO workLog)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            if (workLog.TimeWorked > 24)
            {
                return BadRequest("Can't work more than 24 hours a day.");
            }

            var task = await _taskProjService.GetTaskByIdAsync(workLog.TaskId);
            if (task == null)
            {
                return NotFound("Invalid input, task Id doesn't exist.");
            }

            if (task.AssigneeId != currentUser.Id && task.OwnerId != currentUser.Id)
            {
                return StatusCode(403);
            }

            if (ModelState.IsValid)
            {
                bool isCreated = await _workLogService.CreateWorkLogAsync(workLog.TimeWorked, task.Id);
                if (isCreated)
                {
                    return Ok("Worklog created successfully.");
                }
            }

            return BadRequest("Can't have more than 1 worklog per day.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditWorkLogAsync(int id, WorkLogEditRequestDTO worklogEdit)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            var worklog = await _workLogService.GetWorkLogByIdAsync(id);
            if (worklog == null)
            {
                return NotFound("Invalid input. WorkLog Id doesn't exist.");
            }

            var task = await _taskProjService.GetTaskByIdAsync(worklog.TaskId);
            if (task.AssigneeId != currentUser.Id && task.OwnerId != currentUser.Id)
            {
                return StatusCode(403);
            }

            bool isEdited = await _workLogService.EditWorkLogAsync(id, worklogEdit.TimeWorked);
            if (!isEdited)
            {
                return BadRequest("Can't work more than 12 hours a day.");
            }

            return Ok($"Worklog with Id: {id} was edited successfully.");
        }

        [HttpDelete("{worklogId}")]
        public async Task<IActionResult> DeleteWorkLogAsync(int worklogId)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            var worklog = await _workLogService.GetWorkLogByIdAsync(worklogId);
            if (worklog == null)
            {
                return NotFound("Invalid input. Task Id doesn't exist.");
            }

            var task = await _taskProjService.GetTaskByIdAsync(worklog.TaskId);
            if (task.AssigneeId != currentUser.Id && task.OwnerId != currentUser.Id)
            {
                return StatusCode(403);
            }

            bool isDeleted = await _workLogService.DeleteWorkLogAsync(worklogId);
            if (!isDeleted)
            {
                return BadRequest("WorkLog can't be deleted, please check input");
            }

            return Ok($"WorkLog with Id: {worklogId} was deleted successfully");
        }
    }
}
