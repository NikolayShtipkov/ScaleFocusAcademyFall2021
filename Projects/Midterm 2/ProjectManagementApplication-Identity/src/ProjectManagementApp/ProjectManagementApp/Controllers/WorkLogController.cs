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

            var task = await _taskProjService.GetTaskByIdAsync(taskId);

            var worklogs = await _workLogService.GetWorkLogsFromTasks(taskId, currentUser.Id);
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

            bool isCreated = await _workLogService.CreateWorkLogAsync(workLog.TimeWorked, workLog.TaskId, currentUser.Id);
            if (isCreated && ModelState.IsValid)
            {
                return Ok("Worklog created successfully.");
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditWorkLogAsync(int id, WorkLogEditRequestDTO worklogEdit)
        {
            var currentUser = _userService.GetCurrentUser(Request);

            bool isEdited = await _workLogService.EditWorkLogAsync(id, currentUser.Id,  worklogEdit.TimeWorked);
            if (isEdited)
            {
                return Ok($"Worklog with Id: {id} was edited successfully.");
            }

            return BadRequest();
        }

        [HttpDelete("{worklogId}")]
        public async Task<IActionResult> DeleteWorkLogAsync(int worklogId)
        {
            var currentUser = _userService.GetCurrentUser(Request);

            bool isDeleted = await _workLogService.DeleteWorkLogAsync(worklogId, currentUser.Id);
            if (!isDeleted)
            {
                return BadRequest("WorkLog can't be deleted, please check input");
            }

            return Ok($"WorkLog with Id: {worklogId} was deleted successfully");
        }
    }
}
