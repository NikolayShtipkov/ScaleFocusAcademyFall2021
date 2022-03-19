using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListApp.Services;

namespace ToDoListApp.View
{
    public class UserMenu
    {
        private static UserService _userService;
        public UserMenu(UserService userService)
        {
            _userService = userService;
        }

        public void UsersManagement()
        {
            bool shouldExit = false;
            while (!shouldExit)
            {
                shouldExit = PromptUserMenu();
            }
        }

        public bool PromptUserMenu()
        {
            if (_userService.CurrentUser == null)
            {
                Console.WriteLine("You must first login");
                return true;
            }
            else if (!_userService.CurrentUser.IsAdmin)
            {
                Console.WriteLine("You must be an administrator to use this service");
                return true;
            }

            Console.WriteLine("-------- User Management --------");

            Console.WriteLine("1. Create user");
            Console.WriteLine("2. Edit user");
            Console.WriteLine("3. Delete user");
            Console.WriteLine("4. List all users");
            Console.WriteLine("Back /q");

            string userChoice = Console.ReadLine();

            switch (userChoice)
            {
                case "1":
                    PromptCreateUser();
                    break;
                case "2":
                    PromptEditUser();
                    break;
                case "3":
                    PromptDeleteUser();
                    break;
                case "4":
                    PromptListAllUsers();
                    break;
                case "q":
                    return true;
                default:
                    Console.WriteLine("Unknown Command");
                    break;
            }

            return false;
        }

        public void PromptCreateUser()
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
            answerAdmin = answerAdmin.ToLower();

            bool isEmpty = username == "" || password == "" || firstName == "" || lastName == "";
            if (isEmpty)
            {
                Console.WriteLine("Invalid input, cannot have empty fields");
                return;
            }

            bool isAdmin = false;
            if (answerAdmin == "y")
            {
                isAdmin = true;
            }
            else if (answerAdmin == "n")
            {
                isAdmin = false;
            }
            else
            {
                Console.WriteLine("Invalid input, please use y/n keys");
                return;
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

        public void PromptEditUser()
        {
            Console.WriteLine("Enter user ID:");
            string id = Console.ReadLine();

            Console.WriteLine("Edit username:");
            string username = Console.ReadLine();

            Console.WriteLine("Edit password:");
            string password = Console.ReadLine();

            Console.WriteLine("Edit first name:");
            string firstName = Console.ReadLine();

            Console.WriteLine("Edit last name:");
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

        public void PromptDeleteUser()
        {
            Console.WriteLine("Enter the ID of the user you want to delete:");
            string id = Console.ReadLine();

            int idNum = 0;
            if (!Int32.TryParse(id, out idNum))
            {
                Console.WriteLine("Invalid ID");
            }
            else
            {
                bool isSuccessful = _userService.DeleteUser(idNum);

                if (isSuccessful)
                {
                    Console.WriteLine("User has been deleted");
                }
                else
                {
                    Console.WriteLine("Invalid input, please try again");
                }
            }
        }

        public void PromptListAllUsers()
        {
            _userService.ListAllUsers();
        }
    }
}
