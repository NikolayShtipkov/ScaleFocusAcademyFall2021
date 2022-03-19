using System;
using ToDoListApp.Services;

namespace ToDoListApp.View
{
    public class Menu
    {
        private static UserService _userService = new UserService();
        private static ToDoListService _toDoListService = new ToDoListService(_userService);
        private static TaskService _taskService = new TaskService(_userService, _toDoListService);

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
                    UsersManagement();
                    return false;
                case "exit":
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
        }

        private static void LogOut()
        {
            _userService.LogOut();
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

        private static void UsersManagement()
        {
            if (_userService.CurrentUser == null)
            {
                Console.WriteLine("You must first login");
                return;
            }
            else if (!_userService.CurrentUser.IsAdmin)
            {
                Console.WriteLine("You must be an admin user to use this service");
                return;
            }

            Console.WriteLine("-------- User Management --------");

            Console.WriteLine("1. Create user");
            Console.WriteLine("2. Edit user");
            Console.WriteLine("3. Delete user");
            Console.WriteLine("4. List all users");

            string userChoice = Console.ReadLine();

            switch (userChoice)
            {
                case "1":
                    CreateUser();
                    break;
                case "2":
                    EditUser();
                    break;
                case "4":
                    ListAllUsers();
                    break;
                case "exit":
                    break;
                default:
                    Console.WriteLine("Unknown Command");
                    break;
            }
        }

        private static void CreateUser()
        {
            Console.WriteLine("Enter username:");
            string username = Console.ReadLine();

            Console.WriteLine("Enter password:");
            string password = Console.ReadLine();

            Console.WriteLine("Enter first name:");
            string firstName = Console.ReadLine();

            Console.WriteLine("Enter last name:");
            string lastName = Console.ReadLine();

            Console.WriteLine("Will user be admin y/n:");
            string answerAdmin = Console.ReadLine();

            bool isAdmin = false;

            if (answerAdmin.ToLower() == "y")
            {
                isAdmin = true;
            }

            bool isSuccessful = _userService.CreateUser(username, password, firstName, lastName, isAdmin);
            if (isSuccessful)
            {
                Console.WriteLine("New user created successfully");
            }
            else
            {
                Console.WriteLine("Invalid input, please try again");
            }
        }

        private static void EditUser()
        {

            Console.WriteLine("Enter user ID:");
            string id = Console.ReadLine();

            Console.WriteLine("If you dont want to edit one of the fields leave it blank and press enter");

            Console.WriteLine("Enter username:");
            string username = Console.ReadLine();

            Console.WriteLine("Enter password:");
            string password = Console.ReadLine();

            Console.WriteLine("Enter first name:");
            string firstName = Console.ReadLine();

            Console.WriteLine("Enter last name:");
            string lastName = Console.ReadLine();

            int idNum = 0;
            if (!Int32.TryParse(id, out idNum))
            {
                Console.WriteLine("Invalid ID");
            }
            else
            {
                bool isSuccessful = _userService.EditUser(idNum, username, password, firstName, lastName);

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

        public static void ListAllUsers()
        {
            _userService.ListAllUsers();
        }
    }
}
