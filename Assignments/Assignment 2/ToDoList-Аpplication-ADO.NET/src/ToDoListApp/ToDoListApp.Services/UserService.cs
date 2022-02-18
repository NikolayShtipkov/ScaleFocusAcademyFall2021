using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListApp.Data;
using ToDoListApp.Entities;

namespace ToDoListApp.Services
{
    public class UserService
    {
        private List<User> _applicationUsers = new List<User>();

        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
            List<User> users = _userRepository.GetUsers();

            if (users.Count == 0)
            {
                CreateUser("admin", "adminpassword", "Admin", "Adminov", true);
            }
            else
            {
                _applicationUsers = users;
            }
        }

        private void UpdateUsers()
        {
            List<User> users = _userRepository.GetUsers();

            _applicationUsers = users;
        }

        public bool CreateUser(string username, string password, string firstName, string lastName, bool isAdmin)
        {
            if (_applicationUsers.Any(u => u.Username == username))
            {
                Console.WriteLine("Username is already in use.");
                return false;
            }

            DateTime now = DateTime.Now;

            var user = new User()
            {
                Username = username,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                IsAdmin = isAdmin,
                CreatorId = CurrentUser != null ? CurrentUser.Id : 1,
                ModifierId = CurrentUser != null ? CurrentUser.Id : 1,
                CreatedAt = now,
                ModifiedAt = now
            };

            _userRepository.AddUser(user);

            UpdateUsers();

            return true;
        }

        public bool Login (string username, string password)
        {
            var user = _applicationUsers.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                CurrentUser = user;
                return true;
            }


            return false;
        }

        public void LogOut ()
        {
            CurrentUser = null;
        }

        public User GetUserById (int id)
        {
            return _applicationUsers.FirstOrDefault(u => u.Id == id);
        }

        public bool DeleteUser(int id)
        {
            var user = _applicationUsers.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return false;
            }

            if (_applicationUsers.Any(u => u.CreatorId == id))
            {
                Console.WriteLine("Can't be deleted, user has created one or more members");
                return false;
            }

            if (_userRepository.IsToDoListCreator(id))
            {
                Console.WriteLine("Can't be deleted, user has created one or more ToDoLists");
                return false;
            }

            if (_userRepository.IsTaskCreator(id))
            {
                Console.WriteLine("Can't be deleted, user has created one or more Tasks");
                return false;
            }

            _userRepository.RemoveToDoListSharesByUser(id);
            _userRepository.RemoveAssignedTasksByUser(id);

            _userRepository.RemoveUser(user);
            UpdateUsers();

            return true;
        }

        public bool EditUser(int id, string username, string password, string firstName, string lastName)
        {
            var user = _applicationUsers.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return false;
            }

            if (_applicationUsers.Any(u => u.Username == username) && user.Username != username)
            {
                return false;
            }

            bool isEmpty = username == "" || password == "" || firstName == "" || lastName == "";
            if(isEmpty)
            {
                return false;
            }

            user.Username = username;
            user.Password = password;
            user.FirstName = firstName;
            user.LastName = lastName;
            
            DateTime now = DateTime.Now;

            user.ModifiedAt = now;
            user.ModifierId = CurrentUser.Id;

            _userRepository.EditUser(user);
            UpdateUsers();

            return true;
        }

        public void ListAllUsers()
        {
            string role;
            foreach (var user in _applicationUsers)
            {
                if(user.IsAdmin)
                {
                    role = "Admin";
                } else
                {
                    role = "Regular User";
                }

                Console.WriteLine("=======================================================");
                Console.WriteLine($"User ID: {user.Id}");
                Console.WriteLine($"Username: {user.Username}");
                Console.WriteLine($"Password: {user.Password}");
                Console.WriteLine($"First name: {user.FirstName}");
                Console.WriteLine($"Last name: {user.LastName}");
                Console.WriteLine($"User role: {role}");
                Console.WriteLine($"Date of creation: {user.CreatedAt}");
                Console.WriteLine($"Creator ID: {user.CreatorId}");
                Console.WriteLine($"Date of last change: {user.ModifiedAt}");
                Console.WriteLine($"Last change made by user with ID: {user.ModifierId}");
                Console.WriteLine("=======================================================\n");
            }


        }

        public User CurrentUser { get; private set; }
    }
}
