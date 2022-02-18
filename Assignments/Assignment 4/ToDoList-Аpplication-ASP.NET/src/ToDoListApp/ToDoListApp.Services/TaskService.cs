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

        private readonly DatabaseContext _context;

        private readonly UserService _userService;

        private readonly ToDoListService _toDoListService;

        public TaskService(DatabaseContext context, UserService userService, ToDoListService toDoService)
        {
            _context = context;
            _userService = userService;
            _toDoListService = toDoService;

            List<Task> tasks = _context.Tasks.ToList();

            if (tasks.Count > 0)
            {
                _applicationTasks = tasks;
            }
        }

        private void UpdateTasks()
        {
            List<Task> tasks = _context.Tasks.ToList();

            _applicationTasks = tasks;
        }

        public bool CreateTask(string title, string description, int listId)
        {
            if (_applicationTasks.Any(t => t.Title == title))
            {
                return false;
            }

            var user = _userService.CurrentUser;
            int userId = user.Id;
            DateTime now = DateTime.Now;

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

            _context.Tasks.Add(task);

            _context.SaveChanges();

            UpdateTasks();

            return true;
        }

        private bool IsToDoListShared(int userId, int listId)
        {
            var sharedList = _context.SharedToDoLists.FirstOrDefault(s => s.UserId == userId && s.ListId == listId);

            if (sharedList != null)
            {
                return true;
            }

            return false;
        }

        public bool DeleteTask(int taskId)
        {
            var task = _applicationTasks.FirstOrDefault(l => l.Id == taskId);

            var userId = _userService.CurrentUser.Id;
            if (task.CreatorId == userId)
            {
                DeleteAssignedTasks(taskId);
                _context.Tasks.Remove(task);

                _context.SaveChanges();

                UpdateTasks();

                return true;
            }

            return false;
        }

        private void DeleteAssignedTasks(int taskId)
        {
            foreach (var task in _context.AssignedTasks.ToList().Where(s => s.TaskId == taskId))
            {
                _context.AssignedTasks.Remove(task);
            }
        }

        public bool CompleteTask(int taskId)
        {
            var task = _applicationTasks.FirstOrDefault(l => l.Id == taskId);
            var toDoList = _toDoListService.GetToDoListById(task.ToDoListId);

            if (task != null)
            {
                task.IsComplete = true;

                _context.SaveChanges();

                UpdateTasks();

                return true;
            }

            return false;
        }

        public bool EditTask(int taskId, string title, string description, bool isComplete)
        {
            var task = _applicationTasks.FirstOrDefault(l => l.Id == taskId);
            var user = _userService.CurrentUser;

            if (_applicationTasks.Any(t => t.Title == title) && task.Title != title)
            {
                return false;
            }

            bool isNull = title == null || description == null;
            if (isNull)
            {
                return false;
            }

            task.Title = title;
            task.Description = description;
            task.IsComplete = isComplete;
            task.ModifiedAt = DateTime.Now;
            task.ModifierId = _userService.CurrentUser.Id;

            _context.Tasks.Update(task);

            _context.SaveChanges();

            UpdateTasks();

            return true;
        }

        public bool AssignTask(int userId, int taskId)
        {
            var user = _userService.GetUserById(userId);
            var task = _applicationTasks.FirstOrDefault(l => l.Id == taskId);

            bool isAssigned = IsAssigned(userId, taskId);
            if (isAssigned)
            {
                return false;
            }

            if (_userService.CurrentUser.Id != task.CreatorId)
            {
                return false;
            }

            var toDoList = _toDoListService.GetToDoListById(task.ToDoListId);

            bool isShared = IsToDoListShared(user.Id, toDoList.Id);
            if (!isShared)
            {
                return false;
            }

            AssignedTask assignedTask = new AssignedTask()
            {
                UserId = userId,
                TaskId = taskId
            };

            _context.AssignedTasks.Add(assignedTask);

            _context.SaveChanges();

            return true;
        }

        public bool IsAssigned(int userId, int taskId)
        {
            var assignedTask = _context.AssignedTasks.FirstOrDefault(s => s.UserId == userId && s.TaskId == taskId);
            if (assignedTask != null)
            {
                return true;
            }

            return false;
        }

        public List<Task> GetAllTasks()
        {
            List<Task> allTasks = new List<Task>();
            var user = _userService.CurrentUser;

            foreach (var task in _applicationTasks)
            {
                var toDoList = _toDoListService.GetToDoListById(task.ToDoListId);
                bool isShared = IsToDoListShared(user.Id, toDoList.Id);
                if (task.CreatorId == user.Id || isShared)
                {
                    allTasks.Add(task);
                }
            }

            return allTasks;
        }

        public Entities.Task GetTaskById(int id)
        {
            return _applicationTasks.FirstOrDefault(u => u.Id == id);
        }

        public void ListAllTasks()
        {
            var user = _userService.CurrentUser;
            var tasks = _applicationTasks;

            Console.WriteLine($"Accessible tasks for user: {user.Username}");

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

                var toDoList = _toDoListService.GetToDoListById(task.ToDoListId);
                bool isShared = IsToDoListShared(user.Id, toDoList.Id);

                if (task.CreatorId == user.Id || isShared)
                {
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
        }
    }
}
