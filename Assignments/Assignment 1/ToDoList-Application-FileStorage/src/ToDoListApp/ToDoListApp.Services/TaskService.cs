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
        private const string StoreFileName = "Tasks.json";

        private readonly FileDatabase _storage;

        private readonly List<Task> _applicationTasks = new List<Task>();

        private readonly UserService _userService;

        private readonly ToDoListService _toDoListService;

        public TaskService(UserService userService, ToDoListService toDoService)
        {
            _storage = new FileDatabase();

            _userService = userService;

            _toDoListService = toDoService;

            List<Task> tasksFromFile = _storage.Read<List<Task>>(StoreFileName);

            if (tasksFromFile != null)
            {
                _applicationTasks = tasksFromFile;
            }
        }

        private void SaveToFile()
        {
            _storage.Write(StoreFileName, _applicationTasks);
        }

        public bool CreateTask(string title, string description, int listId)
        {
            if (_applicationTasks.Any(t => t.Title == title))
            {
                return false;
            }

            int uniqueId = 0;
            if (_applicationTasks.Count > 0)
            {
                foreach (var list in _applicationTasks)
                {
                    if (list.Id > uniqueId)
                    {
                        uniqueId = list.Id;
                    }
                }
            }

            uniqueId += 1;
            DateTime now = DateTime.Now;
            int userId = _userService.CurrentUser.Id;
            _applicationTasks.Add(new Task()
            {
                Title = title,
                Description = description,
                IsComplete = false,
                Id = uniqueId,
                ToDoListId = listId,
                CreatorId = userId,
                UpdateById = userId,
                CreatedAt = now,
                UpdatedAt = now
            });

            SaveToFile();

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
                _applicationTasks.Remove(task);
                SaveToFile();
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

            bool isShared = user.SharedToDoListIds.Contains(task.ToDoListId);
            if (task.CreatorId != user.Id && !isShared)
            {
                return false;
            }
            if (_applicationTasks.Any(t => t.Title == title))
            {
                return false;
            }

            if (title != "")
            {
                task.Title = title;
            }
            if (description != "")
            {
                task.Description = description;
            }

            task.IsComplete = isComplete;
            task.UpdatedAt = DateTime.Now;
            task.UpdateById = _userService.CurrentUser.Id;

            SaveToFile();
            return true;
        }

        public bool AssignUser(int userId, int taskId)
        {
            var assaignee = _userService.GetUserById(userId);
            var task = _applicationTasks.FirstOrDefault(l => l.Id == taskId);
            var toDoList = _toDoListService.GetListById(task.ToDoListId);
            if(assaignee == null || task == null) {
                return false;
            }

            if (_userService.CurrentUser.Id != task.CreatorId)
            {
                return false;
            }

            if (task.AssignedUsers.Contains(userId))
            {
                return false;
            }

            bool isShared = assaignee.SharedToDoListIds.Contains(toDoList.Id);
            if (!isShared)
            {
                return false;
            }

            task.AssignedUsers.Add(userId);

            SaveToFile();

            return true;
        }

        public void ListAllTasks()
        {
            var listAll = _applicationTasks;

            foreach (var task in listAll)
            {
                Console.WriteLine($"Task id:                    {task.Id}");
                Console.WriteLine($"ToDoList id:                {task.ToDoListId}");
                Console.WriteLine($"Task name:                  {task.Title}");
                Console.WriteLine($"Task description:           {task.Description}");
                Console.WriteLine($"Task status:                {task.AssignedUsers.Count}");
                Console.WriteLine($"Task date of creation:      {task.CreatedAt}");
                Console.WriteLine($"Task creator id:            {task.CreatorId}");
                Console.WriteLine($"Task date of last change:   {task.UpdatedAt}");
                Console.WriteLine($"Task last modifier id:      {task.UpdateById}\n");
            }
        }
    }
}
