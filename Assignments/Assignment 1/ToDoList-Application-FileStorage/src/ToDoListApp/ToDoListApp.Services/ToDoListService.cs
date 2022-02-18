using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListApp.Data;
using ToDoListApp.Entities;

namespace ToDoListApp.Services
{
    public class ToDoListService
    {
        private const string StoreFileName = "ToDoLists.json";

        private readonly FileDatabase _storage;

        private readonly List<ToDoList> _applicationToDoLists = new List<ToDoList>();

        private readonly UserService _userService;

        public ToDoListService(UserService userService)
        {
            _storage = new FileDatabase();

            _userService = userService;

            List<ToDoList> toDoListsFromFile = _storage.Read<List<ToDoList>>(StoreFileName);

            if (toDoListsFromFile != null)
            {
                _applicationToDoLists = toDoListsFromFile;
            }

        }
        private void SaveToFile()
        {
            _storage.Write(StoreFileName, _applicationToDoLists);
        }

        public bool CreateToDoList(string title)
        {
            if (_applicationToDoLists.Any(t => t.Title == title))
            {
                return false;
            }

            int uniqueId = 0;
            if (_applicationToDoLists.Count > 0)
            {
                foreach (var list in _applicationToDoLists)
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
            _applicationToDoLists.Add(new ToDoList()
            {
                Title = title,
                Id = uniqueId,
                CreatorId = userId,
                UpdateById = userId,
                CreatedAt = now,
                UpdatedAt = now
            });

            _userService.AddToDoListId(userId, uniqueId);

            SaveToFile();

            return true;
        }

        public bool DeleteToDoLIst(int listId)
        {
            var list = _applicationToDoLists.FirstOrDefault(l => l.Id == listId);
            if(list == null)
            {
                return false;
            }


            var userId = _userService.CurrentUser.Id;
            if (list.CreatorId == userId)
            {
                _applicationToDoLists.Remove(list);
                _userService.DeleteToDoListId(userId, listId);
                SaveToFile();
                return true;
            }

            var userShares = _userService.CurrentUser.SharedToDoListIds;
            if (userShares.Contains(listId)) 
            {
                _userService.DeleteShare(userId, listId);
                SaveToFile();
                return true;
            }

            return false;
        }

        public bool EditToDoList(int listId, string title)
        {
            var list = _applicationToDoLists.FirstOrDefault(l => l.Id == listId);
            if (list == null)
            {
                return false;
            }

            bool isShared = _userService.CurrentUser.SharedToDoListIds.Contains(listId);
            if (list.CreatorId != _userService.CurrentUser.Id && !isShared)
            {
                return false;
            }

            if (_applicationToDoLists.Any(t => t.Title == title))
            {
                return false;
            }

            if (title != "")
            {
                list.Title = title;
            }

            list.UpdatedAt = DateTime.Now;
            list.UpdateById = _userService.CurrentUser.Id;

            SaveToFile();
            return true;
        }

        public bool ShareToDoList(int userId, int listId)
        {
            var list = _applicationToDoLists.FirstOrDefault(l => l.Id == listId);
            var user = _userService.GetUserById(userId);
            if (list.CreatorId != _userService.CurrentUser.Id || list == null || user == null)
            {
                return false;
            }

            _userService.ShareToDoListId(userId, listId);

            return true;
        }

        public void ListAllToDoLists()
        {
            var user = _userService.CurrentUser;

            if (user.ToDoListIds.Count > 0)
            {
                Console.WriteLine("Lists created by user: " + user.Username);
                foreach (var id in user.ToDoListIds)
                {
                    var list = _applicationToDoLists.FirstOrDefault(l => l.Id == id);
                    Console.WriteLine("=======================================================");
                    Console.WriteLine($"List id: {list.Id}");
                    Console.WriteLine($"List title: {list.Title}");
                    Console.WriteLine($"Date of creation: {list.CreatedAt}");
                    Console.WriteLine($"Creator id: {list.CreatorId}");
                    Console.WriteLine($"Date of last change: {list.UpdatedAt}");
                    Console.WriteLine($"Last update user ID: {list.UpdateById}");
                    Console.WriteLine("=======================================================\n");
                }
            } 
            else
            {
                Console.WriteLine("No created lists");
            }

            if (user.SharedToDoListIds.Count > 0)
            {
                Console.WriteLine("Shared lists");
                foreach (var id in user.SharedToDoListIds)
                {
                    var list = _applicationToDoLists.FirstOrDefault(l => l.Id == id);

                    if(list != null)
                    {
                        Console.WriteLine("=======================================================");
                        Console.WriteLine($"List id: {list.Id}");
                        Console.WriteLine($"List title: {list.Title}");
                        Console.WriteLine($"Date of creation: {list.CreatedAt}");
                        Console.WriteLine($"Creator id: {list.CreatorId}");
                        Console.WriteLine($"Date of last change: {list.UpdatedAt}");
                        Console.WriteLine($"Last update user ID: {list.UpdateById}");
                        Console.WriteLine("=======================================================\n");
                    }

                }
            } 
            else
            {
                Console.WriteLine("No shared lists");
            }
        }

        public ToDoList GetListById(int id)
        {
            return _applicationToDoLists.FirstOrDefault(u => u.Id == id);
        }
    }
}
