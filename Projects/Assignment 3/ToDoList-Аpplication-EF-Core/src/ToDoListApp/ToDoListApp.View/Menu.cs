using System;
using ToDoListApp.Data;
using ToDoListApp.Services;

namespace ToDoListApp.View
{
    public class Menu
    {
        private static UserService _userService;
        private static ToDoListService _toDoListService;
        private static TaskService _taskService;

        private static UserMenu _userMenu;
        private static ToDoListMenu _toDoListMenu;
        private static TaskMenu _taskMenu;

        public Menu(UserService userService, ToDoListService toDoListService, TaskService taskService, UserMenu userMenu, ToDoListMenu toDoListMenu, TaskMenu taskMenu)
        {
            _userService = userService;
            _toDoListService = toDoListService;
            _taskService = taskService;

            _userMenu = userMenu;
            _toDoListMenu = toDoListMenu;
            _taskMenu = taskMenu;
        }

        public bool MainMenu()
        {
            RenderMenu();

            string userChoice = Console.ReadLine();
            switch (userChoice)
            {
                case "1":
                    if (_userService.CurrentUser == null)
                    {
                        LogIn();
                    }
                    else
                    {
                        LogOut();
                    }
                    return false;
                case "2":
                    _userMenu.UsersManagement();
                    return false;
                case "3":
                    _toDoListMenu.ToDoListManagement();
                    return false;
                case "4":
                    _taskMenu.TaskManagement();
                    return false;
                case "q":
                    return true;
                default:
                    Console.WriteLine("Unknown Command");
                    return false;
            }
        }

        private static void RenderMenu()
        {
            Console.WriteLine("--------Main Menu--------");
            if (_userService.CurrentUser == null)
            {
                Console.WriteLine("1. LogIn ");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"You are logged in as: {_userService.CurrentUser.Username}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("1. LogOut");
            }
            Console.WriteLine("2. Users Management");
            Console.WriteLine("3. ToDo Lists Management");
            Console.WriteLine("4. Task Management");
            Console.WriteLine("Quit /q");
        }

        private static void LogIn()
        {
            Console.WriteLine("Enter your user name:");
            string username = Console.ReadLine();

            Console.WriteLine("Enter your password:");
            string password = Console.ReadLine();

            _userService.Login(username, password);

            if (_userService.CurrentUser == null)
            {
                Console.WriteLine("Login failed.");
            }
            else
            {
                Console.WriteLine("Login successful.");
            }
        }

        private static void LogOut()
        {
            _userService.LogOut();
        }
    }
}
