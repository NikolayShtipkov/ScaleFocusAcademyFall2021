using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListApp.Entities;
using ToDoListApp.Data;
using Task = ToDoListApp.Entities.Task;

namespace ToDoListApp.Services
{
    public class TaskService
    {
        private List<Task> _applicationTasks = new List<Task>();

        private readonly TaskRepository _taskRepository;

        private readonly UserService _userService;

        private readonly ToDoListService _toDoListService;

        public TaskService(TaskRepository taskRepository, UserService userService, ToDoListService toDoService)
        {
            _taskRepository = taskRepository;
            _userService = userService;
            _toDoListService = toDoService;
            List<Task> tasks = _taskRepository.GetTasks();

            if (tasks.Count > 0)
            {
                _applicationTasks = tasks;
            }
        }

        private void UpdateTasks()
        {
            List<Task> tasks = _taskRepository.GetTasks();

            _applicationTasks = tasks;
        }

        public bool CreateTask(string title, string description, int listId)
        {
            if (_applicationTasks.Any(t => t.Title == title))
            {
                Console.WriteLine("Title has been taken.");
                return false;
            }

            var user = _userService.CurrentUser;
            if (user == null)
            {
                return false;
            }

            var toDoList = _toDoListService.GetListById(listId);
            bool isShared = _toDoListService.CheckShare(user.Id, listId);
            if (toDoList.CreatorId != user.Id && !isShared)
            {
                return false;
            }

            DateTime now = DateTime.Now;
            int userId = user.Id;

            var task = new Task()
            {
                Title = title,
                Description = description,
                IsComplete = false,
                ToDoListId = listId,
                CreatorId = userId,
                ModifierId = userId,
                CreatedAt = now,
                ModifiedAt = now
            };

            _taskRepository.AddTask(task);
            UpdateTasks();

            return true;
        }

        public bool DeleteTask(int taskId)
        {
            var task = _applicationTasks.FirstOrDefault(l => l.Id == taskId);
            if (task == null)
            {
                return false;
            }

            var userId = _userService.CurrentUser.Id;
            if (task.CreatorId == userId)
            {
                _taskRepository.RemoveAssignedUsers(taskId);
                _taskRepository.RemoveTask(task);
                UpdateTasks();

                return true;
            }

            return false;
        }

        public bool CompleteTask(int taskId)
        {
            var task = _applicationTasks.FirstOrDefault(l => l.Id == taskId);
            if (task != null)
            {
                task.IsComplete = true;
                _taskRepository.CompleteTask(task);
                UpdateTasks();

                return true;
            }

            return false;
        }

        public bool EditTask(int taskId, string title, string description, bool isComplete)
        {
            var task = _applicationTasks.FirstOrDefault(l => l.Id == taskId);
            var user = _userService.CurrentUser;
            if (task == null || user == null)
            {
                return false;
            }

            var toDoList = _toDoListService.GetListById(task.ToDoListId);
            bool isShared = _toDoListService.CheckShare(user.Id, toDoList.Id);
            if (task.CreatorId != user.Id && !isShared)
            {
                return false;
            }

            if (_applicationTasks.Any(t => t.Title == title) && task.Title != title)
            {
                return false;
            }

            bool isEmpty = title == "" || description == "";
            if (isEmpty)
            {
                return false;
            }

            task.Title = title;
            task.Description = description;
            task.IsComplete = isComplete;
            task.ModifiedAt = DateTime.Now;
            task.ModifierId = _userService.CurrentUser.Id;

            _taskRepository.EditTask(task);
            UpdateTasks();

            return true;
        }

        public bool AssignUser(int userId, int taskId)
        {
            var user = _userService.GetUserById(userId);
            var task = _applicationTasks.FirstOrDefault(l => l.Id == taskId);
            if (user == null || task == null)
            {
                return false;
            }


            if (_taskRepository.IsAssigned(userId, taskId))
            {
                return false;
            }

            if (_userService.CurrentUser.Id != task.CreatorId)
            {
                return false;
            }

            var toDoList = _toDoListService.GetListById(task.ToDoListId);

            bool isShared = _toDoListService.CheckShare(user.Id, toDoList.Id);
            if (!isShared)
            {
                return false;
            }

            
            _taskRepository.AssignTask(user, task);

            return true;
        }

        public void ListAllTasks()
        {
            var tasks = _applicationTasks;

            if (tasks != null)
            {
                foreach (var task in tasks)
                {
                    string status;
                    if (!task.IsComplete)
                    {
                        status = "Not Completed";
                    }
                    else
                    {
                        status = "Completed";
                    }

                    Console.WriteLine("=======================================================");
                    Console.WriteLine($"Task id: {task.Id}");
                    Console.WriteLine($"ToDoList id: {task.ToDoListId}");
                    Console.WriteLine($"Title: {task.Title}");
                    Console.WriteLine($"Description: {task.Description}");
                    Console.WriteLine($"Task status: {status}");
                    Console.WriteLine($"Creator id: {task.CreatorId}");
                    Console.WriteLine($"Date of creation: {task.CreatedAt}");
                    Console.WriteLine($"Last modifier id: {task.ModifierId}");
                    Console.WriteLine($"Date of last change: {task.ModifiedAt}");
                    Console.WriteLine("=======================================================\n");
                }
            }
            else
            {
                Console.WriteLine("No tasks created");
            }
        }
    }
}
