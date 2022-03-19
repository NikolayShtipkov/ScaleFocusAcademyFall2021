using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListApp.Services;

namespace ToDoListApp.View
{
    public class ToDoListMenu
    {
        private static ToDoListService _toDoListService;
        private static UserService _userService;
        public ToDoListMenu(UserService userService, ToDoListService toDoListService)
        {
            _userService = userService;
            _toDoListService = toDoListService;
        }

        public void ToDoListManagement()
        {
            bool shouldExit = false;
            while (!shouldExit)
            {
                shouldExit = PromptToDoListMenu();
            }
        }

        public bool PromptToDoListMenu()
        {
            if (_userService.CurrentUser == null)
            {
                Console.WriteLine("You must first login");
                return true;
            }

            Console.WriteLine("-------- ToDo List Management --------");

            Console.WriteLine("1. Create ToDo List");
            Console.WriteLine("2. Edit ToDo List");
            Console.WriteLine("3. Delete ToDo List");
            Console.WriteLine("4. List all ToDo Lists");
            Console.WriteLine("5. Share ToDo List");
            Console.WriteLine("Back /q");

            string userChoice = Console.ReadLine();

            switch (userChoice)
            {
                case "1":
                    PromtCreateToDoList();
                    break;
                case "2":
                    PromptEditToDoList();
                    break;
                case "3":
                    PromptDeleteToDoList();
                    break;
                case "4":
                    PromptListAllToDoLists();
                    break;
                case "5":
                    PromptShareToDoList();
                    break;
                case "q":
                    return true;
                default:
                    Console.WriteLine("Unknown Command");
                    break;
            }

            return false;
        }

        public void PromtCreateToDoList()
        {
            Console.WriteLine("Enter Title:");
            string title = Console.ReadLine();

            if (title == "")
            {
                Console.WriteLine("Can't have an empty title");
                return;
            }

            bool isSuccessful = _toDoListService.CreateToDoList(title);
            if (isSuccessful)
            {
                Console.WriteLine("ToDoList Created successfuly");
            }
            else
            {
                Console.WriteLine("Invalid input, please try again");
            }
        }

        public void PromptEditToDoList()
        {
            Console.WriteLine("Enter list ID:");
            string id = Console.ReadLine();

            int idNum = 0;
            bool isNum = Int32.TryParse(id, out idNum);

            if (!isNum)
            {
                Console.WriteLine("Invalid ID");
            }
            else
            {
                Console.WriteLine("Edit title:");
                string title = Console.ReadLine();

                bool isSuccessful = _toDoListService.EditToDoList(idNum, title);
                if (isSuccessful)
                {
                    Console.WriteLine("Edit has been successful");
                }
                else
                {
                    Console.WriteLine("Invalid input, please try again");
                }
            }
        }

        private static void PromptDeleteToDoList()
        {
            Console.WriteLine("Enter the ID of the ToDo List you want to delete:");
            string id = Console.ReadLine();

            int idNum = 0;
            if (!Int32.TryParse(id, out idNum))
            {
                Console.WriteLine("Invalid ID");
            }
            else
            {
                bool isSuccessful = _toDoListService.DeleteToDoLIst(idNum);
                if (isSuccessful)
                {
                    Console.WriteLine("Operation successful");
                }
                else
                {
                    Console.WriteLine("Invalid input, please try again");
                }
            }
        }

        public void PromptShareToDoList()
        {
            Console.WriteLine("Enter user ID to share:");
            string userId = Console.ReadLine();

            Console.WriteLine("Enter list ID you want to share:");
            string listId = Console.ReadLine();

            int userIdNum = 0;
            int listIdNum = 0;
            bool isNum = !Int32.TryParse(userId, out userIdNum) || !Int32.TryParse(listId, out listIdNum);
            if (isNum)
            {
                Console.WriteLine("Invalid user or list ID");
            }
            else
            {
                bool isSuccessful = _toDoListService.ShareToDoList(userIdNum, listIdNum);
                if (isSuccessful)
                {
                    Console.WriteLine("ToDo List has been shared");
                }
                else
                {
                    Console.WriteLine("Invalid input, please try again");
                }
            }
        }

        public void PromptListAllToDoLists()
        {
            _toDoListService.ListAllToDoLists();
        }
    }
}
