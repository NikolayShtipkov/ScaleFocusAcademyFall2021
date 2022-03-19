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
        private const string StoreFileName = "Users.json";

        private readonly FileDatabase _storage;

        private readonly List<User> _applicationUsers = new List<User>();

        public UserService()
        {
            _storage = new FileDatabase();
            List<User> usersFromFile = _storage.Read<List<User>>(StoreFileName);

            if (usersFromFile == null)
            {
                CreateUser("admin", "adminpassword", "Admin", "Adminov", true);
            }
            else
            {
                _applicationUsers = usersFromFile;
            }
        }

        private void SaveToFile()
        {
            _storage.Write(StoreFileName, _applicationUsers);
        }

        public bool CreateUser(string username, string password, string firstName, string lastName, bool isAdmin)
        {
            if (_applicationUsers.Any(u => u.Username == username))
            {
                return false;
            }

            int uniqueId = 0;
            if (_applicationUsers.Count > 0)
            {
                foreach (var user in _applicationUsers)
                {
                    if (user.Id > uniqueId)
                    {
                        uniqueId = user.Id;
                    }
                }
            }

            uniqueId += 1;
            DateTime now = DateTime.Now;
            _applicationUsers.Add(new User()
            {
                Username = username,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                IsAdmin = isAdmin,
                Id = uniqueId,
                CreatorId = CurrentUser != null ? CurrentUser.Id : 1,
                UpdateById = CurrentUser != null ? CurrentUser.Id : 1,
                CreatedAt = now,
                UpdatedAt = now
            });

            SaveToFile ();

            return true;
        }

        public bool Login (string username, string password)
        {
            var user = _applicationUsers.FirstOrDefault (u => u.Username == username && u.Password == password);
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
            return _applicationUsers.FirstOrDefault (u => u.Id == id);
        }

        public void DeleteUser(int id)
        {
            var user = _applicationUsers.FirstOrDefault (u => u.Id == id);

            _applicationUsers.Remove(user);
            
            SaveToFile();
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

            if (username != "")
            {
                user.Username = username;
            }
            if (password != "")
            {
                user.Password = password;
            }
            if (firstName != "")
            {
                user.FirstName = firstName;
            }
            if (lastName != "")
            {
                user.LastName = lastName;
            }

            DateTime now = DateTime.Now;

            user.UpdatedAt = now;

            user.UpdateById = CurrentUser.Id;

            SaveToFile();

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
                Console.WriteLine($"Date of last change: {user.UpdatedAt}");
                Console.WriteLine($"Last change made by user with ID: {user.UpdateById}");
                Console.WriteLine("=======================================================\n");
            }
        }

        public void AddToDoListId(int userId, int listId)
        {
            var user = _applicationUsers.FirstOrDefault(u => u.Id == userId);
            user.ToDoListIds.Add(listId);

            SaveToFile();
        }

        public void DeleteToDoListId(int userId, int listId)
        {
            var user = _applicationUsers.FirstOrDefault(u => u.Id == userId);
            if (user.ToDoListIds.Contains(listId))
            {
                user.ToDoListIds.Remove(listId);

                SaveToFile();
            }
        }

        public void ShareToDoListId(int userId, int listId)
        {
            var user = _applicationUsers.FirstOrDefault(u => u.Id == userId);
            if (!user.SharedToDoListIds.Contains(listId))
            {
                user.SharedToDoListIds.Add(listId);

                SaveToFile();
            }
        }

        public void DeleteShare(int userId, int listId)
        {
            var user = _applicationUsers.FirstOrDefault(u => u.Id == userId);
            user.SharedToDoListIds.Remove(listId);

            SaveToFile();
        }

        public User CurrentUser { get; private set; }
    }
}
