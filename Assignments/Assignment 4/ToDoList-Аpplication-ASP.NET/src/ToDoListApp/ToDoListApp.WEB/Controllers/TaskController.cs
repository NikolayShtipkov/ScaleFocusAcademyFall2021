using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Auth;
using ToDoListApp.Data;
using ToDoListApp.Entities;
using ToDoListApp.Models.DTO.Requests;
using ToDoListApp.Models.DTO.Responses;
using ToDoListApp.Services;

namespace ToDoListApp.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private static UserService _userService;
        private static ToDoListService _toDoListService;
        private static TaskService _taskService;

        public TaskController(UserService userService, ToDoListService toDoListService, TaskService taskService) : base()
        {
            _userService = userService;
            _toDoListService = toDoListService;
            _taskService = taskService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TaskResponseDTO>> GetAllTasksFromList()
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            var tasks = _taskService.GetAllTasks();
            if (tasks == null)
            {
                return StatusCode(403);
            }

            if (tasks.Count() == 0)
            {
                return Ok("No tasks from created or shared ToDo lists.");
            }

            var responseTasks = new List<TaskResponseDTO>();
            foreach (var task in tasks)
            {
                responseTasks.Add(MapTask(task));
            }

            return responseTasks;
        }

        [HttpPost]
        public ActionResult CreateTask(TaskCreateRequestDTO task)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            var toDoList = _toDoListService.GetToDoListById(task.ToDoListId);
            if (toDoList == null)
            {
                return NotFound("Invalid input. ToDoList Id doesn't exist.");
            }

            bool hasAccess = _toDoListService.IsToDoListShared(currentUser.Id, toDoList.Id) || currentUser.Id == toDoList.CreatorId;
            if (!hasAccess)
            {
                return StatusCode(403);
            }

            if (ModelState.IsValid)
            {
                bool isCreated = _taskService.CreateTask(task.Title, task.Description, task.ToDoListId);
                if (isCreated)
                {
                    return Ok("Task created successfully.");
                }
            }
            
            return Unauthorized("Title is already in use.");
        }

        [HttpPut("{id}")]
        public ActionResult EditTask(int id, TaskEditRequestDTO taskEdit)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            var task = _taskService.GetTaskById(id);
            if (task == null)
            {
                return BadRequest("Invalid input. Task Id doesn't exist.");
            }

            var toDoList = _toDoListService.GetToDoListById(task.ToDoListId);
            if (toDoList == null)
            {
                return NotFound("Invalid input. ToDoList Id doesn't exist.");
            }

            bool hasAccess = _toDoListService.IsToDoListShared(currentUser.Id, toDoList.Id) || currentUser.Id == toDoList.CreatorId;
            if (!hasAccess)
            {
                return Unauthorized("You don't have accsses to this ToDoList.");
            }

            bool isEdited = _taskService.EditTask(id, taskEdit.Title, taskEdit.Description, taskEdit.IsComplete);
            if (!isEdited)
            {
                return Unauthorized("Task could not be edited.");
            }

            return Ok($"Task with Id: {id} edited successfully.");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            var task = _taskService.GetTaskById(id);
            if (task == null)
            {
                return BadRequest("Invalid input. Task Id doesn't exist.");
            }

            bool isDeleted = _taskService.DeleteTask(id);
            if (!isDeleted)
            {
                return Unauthorized("Task could not be deleted, please check input.");
            }

            return Ok($"Task with Id: {id} deleted successfully.");
        }

        [HttpPost("{taskId}/Assign/{userId}")]
        public ActionResult AssignTask(int taskId, int userId)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            var user = _userService.GetUserById(userId);
            if (user == null)
            {
                return BadRequest("Invalid input. User Id doesn't exist.");
            }

            var task = _taskService.GetTaskById(taskId);
            if (task == null)
            {
                return BadRequest("Invalid input. Task Id doesn't exist.");
            }

            bool hasAssign = _taskService.IsAssigned(userId, taskId);
            if (hasAssign)
            {
                return BadRequest("Task has been already assigned to user.");
            }

            bool isAssigned = _taskService.AssignTask(userId, taskId);
            if (!isAssigned)
            {
                return BadRequest("Task could not be assigned, please check input.");
            }

            return Ok($"Task with Id: {taskId} assigned to user with Id: {userId} successfully.");
        }

        [HttpPut("Complete/{id}")]
        public ActionResult CompleteTask(int id)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            var task = _taskService.GetTaskById(id);
            if (task == null)
            {
                return BadRequest("Invalid input. Task Id doesn't exist.");
            }

            var toDoList = _toDoListService.GetToDoListById(task.ToDoListId);
            if (toDoList == null)
            {
                return NotFound("Invalid input. ToDoList Id doesn't exist.");
            }

            bool hasAccess = _toDoListService.IsToDoListShared(currentUser.Id, toDoList.Id) || currentUser.Id == toDoList.CreatorId;
            if (!hasAccess)
            {
                return StatusCode(403);
            }

            bool isCompleted = _taskService.CompleteTask(id);
            if (!isCompleted)
            {
                return Unauthorized("Task could not be completed, please check input.");
            }

            return Ok("Task has been completed.");
        }

        private TaskResponseDTO MapTask(Entities.Task task)
        {
            TaskResponseDTO responseTask = new TaskResponseDTO()
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsComplete = task.IsComplete,
                ToDoListId = task.ToDoListId,
                CreatorId = task.CreatorId,
                ModifierId = task.ModifierId,
                CreatedAt = task.CreatedAt,
                ModifiedAt = task.ModifiedAt
            };
            return responseTask;
        }
    }
}
