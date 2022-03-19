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
        private List<ToDoList> _applicationToDoLists = new List<ToDoList>();

        private readonly ToDoListRepository _toDoListRepository;

        private readonly UserService _userService;

        public ToDoListService(ToDoListRepository toDoListRepository, UserService userService)
        {
            _toDoListRepository = toDoListRepository;
            _userService = userService;
            List<ToDoList> toDoLists = _toDoListRepository.GetToDoLists();

            if (toDoLists.Count > 0)
            {
                _applicationToDoLists = toDoLists;
            }

        }

        private void UpdateToDoLists()
        {
            List<ToDoList> toDoLists = _toDoListRepository.GetToDoLists();

            _applicationToDoLists = toDoLists;
        }

        public bool CreateToDoList(string title)
        {
            if (_applicationToDoLists.Any(t => t.Title == title))
            {
                Console.WriteLine("Title has been taken");
                return false;
            }

            DateTime now = DateTime.Now;
            int userId = _userService.CurrentUser.Id;

            var toDoList = new ToDoList()
            {
                Title = title,
                CreatorId = userId,
                ModifierId = userId,
                CreatedAt = now,
                ModifiedAt = now
            };

            _toDoListRepository.AddToDoList(toDoList);
            UpdateToDoLists();

            return true;
        }

        public bool DeleteToDoLIst(int listId)
        {
            var list = _applicationToDoLists.FirstOrDefault(l => l.Id == listId);
            if(list == null)
            {
                return false;
            }

            if (!_toDoListRepository.IsToDoListEmpty(listId))
            {
                Console.WriteLine("Can't be deleted, this ToDoList has one or more tasks");
                return false;
            }

            var userId = _userService.CurrentUser.Id;
            if (list.CreatorId == userId)
            {
                _toDoListRepository.RemoveSharedToDoListsById(listId);

                _toDoListRepository.RemoveToDoList(list);

                UpdateToDoLists();

                return true;
            }

            _toDoListRepository.RemoveToDoListShare(userId, listId);

            return false;
        }

        public bool EditToDoList(int listId, string title)
        {
            var list = _applicationToDoLists.FirstOrDefault(l => l.Id == listId);
            if (list == null)
            {
                return false;
            }


            bool isShared = _toDoListRepository.IsShared(_userService.CurrentUser.Id, list.Id);
            if (list.CreatorId != _userService.CurrentUser.Id && !isShared)
            {
                return false;
            }

            if (_applicationToDoLists.Any(t => t.Title == title) && list.Title != title)
            {
                return false;
            }

            if (title == "")
            {
                return false;
            }

            list.Title = title;
            list.ModifiedAt = DateTime.Now;
            list.ModifierId = _userService.CurrentUser.Id;

            _toDoListRepository.EditToDoList(list);
            UpdateToDoLists();

            return true;
        }

        public bool ShareToDoList(int userId, int listId)
        {
            var user = _userService.GetUserById(userId);
            var list = _applicationToDoLists.FirstOrDefault(l => l.Id == listId);
            if (list == null|| list.CreatorId != _userService.CurrentUser.Id || user == null || user == _userService.CurrentUser)
            {
                return false;
            }

            _toDoListRepository.ShareToDoList(user, list);

            return true;
        }

        public void ListAllToDoLists()
        {
            Console.WriteLine("Created ToDoLists:\n");

            foreach (var list in _applicationToDoLists)
            {
                if (_userService.CurrentUser.Id == list.CreatorId)
                {
                    Console.WriteLine("=======================================================");
                    Console.WriteLine($"List id: {list.Id}");
                    Console.WriteLine($"List title: {list.Title}");
                    Console.WriteLine($"Date of creation: {list.CreatedAt}");
                    Console.WriteLine($"Creator id: {list.CreatorId}");
                    Console.WriteLine($"Date of last change: {list.ModifiedAt}");
                    Console.WriteLine($"Last update user ID: {list.ModifierId}");
                    Console.WriteLine("=======================================================\n");
                }
            }

            Console.WriteLine("ToDoLists shared with you:\n");

            var sharedLists = _toDoListRepository.GetSharedToDoLists();

            foreach (var item in sharedLists)
            {
                if (_userService.CurrentUser.Id == item.UserId)
                {
                    var list = GetListById(item.ListId);

                    Console.WriteLine("=======================================================");
                    Console.WriteLine($"List id: {list.Id}");
                    Console.WriteLine($"List title: {list.Title}");
                    Console.WriteLine($"Date of creation: {list.CreatedAt}");
                    Console.WriteLine($"Creator id: {list.CreatorId}");
                    Console.WriteLine($"Date of last change: {list.ModifiedAt}");
                    Console.WriteLine($"Last update user ID: {list.ModifierId}");
                    Console.WriteLine("=======================================================\n");
                }
            }
        }

        public bool CheckShare(int user, int toDoList)
        {
            return _toDoListRepository.IsShared(user, toDoList);
        }
        public ToDoList GetListById(int id)
        {
            return _applicationToDoLists.FirstOrDefault(u => u.Id == id);
        }

    }
}
