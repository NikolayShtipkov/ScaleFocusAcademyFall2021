using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListApp.Services;

namespace ToDoListApp.View
{
    public class TaskMenu
    {
        private static ToDoListService _toDoListService;
        private static UserService _userService;
        private static TaskService _taskService;
        public TaskMenu(UserService userService, ToDoListService toDoListService, TaskService taskService)
        {
            _userService = userService;
            _toDoListService = toDoListService;
            _taskService = taskService;
        }

        public void TaskManagement()
        {
            bool shouldExit = false;
            while (!shouldExit)
            {
                shouldExit = PromptTaskMenu();
            }
        }

        public bool PromptTaskMenu()
        {
            if (_userService.CurrentUser == null)
            {
                Console.WriteLine("You must first login");
                return true;
            }

            Console.WriteLine("-------- Task Management --------");

            Console.WriteLine("1. Create task");
            Console.WriteLine("2. Edit task");
            Console.WriteLine("3. Delete task");
            Console.WriteLine("4. List all tasks");
            Console.WriteLine("5. Complete task");
            Console.WriteLine("6. Assign task");
            Console.WriteLine("Back /q");

            string userChoice = Console.ReadLine();

            switch (userChoice.ToLower())
            {
                case "1":
                    PromptCreateTask();
                    break;
                case "2":
                    PromptEditTask();
                    break;
                case "3":
                    PromptDeleteTask();
                    break;
                case "4":
                    PromptListAllTasks();
                    break;
                case "5":
                    PromptCompleteTask();
                    break;
                case "6":
                    PromptAssignUser();
                    break;
                case "q":
                    return true;
                default:
                    Console.WriteLine("Unknown Command");
                    break;
            }

            return false;
        }

        public void PromptCreateTask()
        {
            Console.WriteLine("Enter Title:");
            string title = Console.ReadLine();

            Console.WriteLine("Write down task description:");
            string description = Console.ReadLine();

            Console.WriteLine("Enter ToDo list ID:");
            string listId = Console.ReadLine();

            bool isEmpty = title == "" || description == "" || listId == "";
            if (isEmpty)
            {
                Console.WriteLine("Invalid Input, can't have empty fields.");
                return;
            }

            int idNum = 0;
            if (!Int32.TryParse(listId, out idNum))
            {
                Console.WriteLine("Invalid ID");
            }
            else
            {
                bool isSuccessful = _taskService.CreateTask(title, description, idNum);
                if (isSuccessful)
                {
                    Console.WriteLine("Task Created successfuly");
                }
                else
                {
                    Console.WriteLine("Invalid input, please try again");
                }
            }
        }

        public void PromptEditTask()
        {
            Console.WriteLine("Enter task ID:");
            string taskId = Console.ReadLine();

            Console.WriteLine("Edit title:");
            string title = Console.ReadLine();

            Console.WriteLine("Edit description:");
            string description = Console.ReadLine();

            Console.WriteLine("Is the task completed y/n:");
            string answerAdmin = Console.ReadLine();

            int idNum = 0;
            if (!Int32.TryParse(taskId, out idNum))
            {
                Console.WriteLine("Invalid ID");
            }

            bool isComplete = false;
            if (answerAdmin.ToLower() == "y")
            {
                isComplete = true;
            }

            bool isSuccessful = _taskService.EditTask(idNum, title, description, isComplete);
            if (isSuccessful)
            {
                Console.WriteLine("Task edited");
            }
            else
            {
                Console.WriteLine("Invalid input, please try again");
            }
        }

        public void PromptDeleteTask()
        {
            Console.WriteLine("Enter the ID of the Task you want to delete:");
            string taskId = Console.ReadLine();

            int idNum = 0;
            if (!Int32.TryParse(taskId, out idNum))
            {
                Console.WriteLine("Invalid task ID");
            }
            else
            {
                bool isSuccessful = _taskService.DeleteTask(idNum);
                if (isSuccessful)
                {
                    Console.WriteLine("Task has been deleted");
                }
                else
                {
                    Console.WriteLine("Invalid input, please try again");
                }
            }
        }

        public void PromptCompleteTask()
        {
            Console.WriteLine("Enter the ID of the Task you want to complete:");
            string taskId = Console.ReadLine();

            int idNum = 0;
            if (!Int32.TryParse(taskId, out idNum))
            {
                Console.WriteLine("Invalid task ID");
            }
            else
            {
                bool isSuccessful = _taskService.CompleteTask(idNum);
                if (isSuccessful)
                {
                    Console.WriteLine("Task has been completed");
                }
                else
                {
                    Console.WriteLine("Invalid input, please try again");
                }
            }
        }

        public void PromptAssignUser()
        {
            Console.WriteLine("Enter user ID:");
            string userId = Console.ReadLine();

            Console.WriteLine("Enter task ID:");
            string taskId = Console.ReadLine();

            int userIdNum = 0;
            int listIdNum = 0;

            bool isNum = !Int32.TryParse(userId, out userIdNum) || !Int32.TryParse(taskId, out listIdNum);
            if (isNum)
            {
                Console.WriteLine("Invalid user or list ID");
            }
            else
            {
                bool isSuccessful = _taskService.AssignUser(userIdNum, listIdNum);
                if (isSuccessful)
                {
                    Console.WriteLine("Task has been assigned");
                }
                else
                {
                    Console.WriteLine("Invalid input, please try again");
                }
            }
        }

        public void PromptListAllTasks()
        {
            _taskService.ListAllTasks();
        }
    }
}
