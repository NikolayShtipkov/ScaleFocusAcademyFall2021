using Microsoft.EntityFrameworkCore;
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

        private readonly DatabaseContext _context;

        private readonly UserService _userService;

        public ToDoListService(DatabaseContext toDoListRepository, UserService userService)
        {
            _context = toDoListRepository;
            _userService = userService;

            List<ToDoList> toDoLists = _context.ToDoLists.ToList();

            if (toDoLists.Count > 0)
            {
                _applicationToDoLists = toDoLists;
            }

        }

        private void UpdateToDoLists()
        {
            List<ToDoList> toDoLists = _context.ToDoLists.ToList();

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

            _context.ToDoLists.Add(toDoList);

            _context.SaveChanges();

            UpdateToDoLists();

            return true;
        }

        public bool DeleteToDoLIst(int listId)
        {
            var list = _applicationToDoLists.FirstOrDefault(l => l.Id == listId);
            var userId = _userService.CurrentUser.Id;
            var shareToDelete = _context.SharedToDoLists
                .FirstOrDefault(s => s.UserId == userId && s.ListId == listId);

            if (shareToDelete != null && userId != list.CreatorId)
            {
                _context.SharedToDoLists.Remove(shareToDelete);

                _context.SaveChanges();

                UpdateToDoLists();

                return true;
            }

            if (_context.Tasks.Any(t => t.ToDoListId == listId))
            {
                Console.WriteLine("Can't be deleted, this ToDoList has one or more tasks");
                return false;
            }

            if (list.CreatorId == userId)
            {
                DeleteSharedToDoLists(listId);

                _context.ToDoLists.Remove(list);

                if (shareToDelete != null)
                {
                    _context.SharedToDoLists.Remove(shareToDelete);
                }
                _context.SaveChanges();

                UpdateToDoLists();

                return true;
            }

            return false;
        }

        public void DeleteSharedToDoLists(int listId)
        {
            foreach (var list in _context.SharedToDoLists.ToList().Where(s => s.ListId == listId))
            {
                _context.SharedToDoLists.Remove(list);
            }
        }

        public bool EditToDoList(int listId, string title)
        {
            var list = _applicationToDoLists.FirstOrDefault(l => l.Id == listId);
            if (_applicationToDoLists.Any(t => t.Title == title) && list.Title != title)
            {
                return false;
            }

            list.Title = title;
            list.ModifiedAt = DateTime.Now;
            list.ModifierId = _userService.CurrentUser.Id;

            _context.ToDoLists.Update(list);

            _context.SaveChanges();

            UpdateToDoLists();

            return true;
        }

        public bool ShareToDoList(int userId, int listId)
        {
            var user = _userService.GetUserById(userId);
            var list = _applicationToDoLists.FirstOrDefault(l => l.Id == listId);
            if (list.CreatorId != _userService.CurrentUser.Id)
            {
                return false;
            }

            SharedToDoList sharedList = new SharedToDoList() 
            {
                UserId = userId,
                ListId = listId
            };

            _context.SharedToDoLists.Add(sharedList);

            _context.SaveChanges();

            return true;
        }

        public bool IsToDoListShared(int userId, int listId)
        {
            var sharedList = _context.SharedToDoLists.FirstOrDefault(s => s.UserId == userId && s.ListId == listId);
            if (sharedList != null)
            {
                return true;
            }

            return false;
        }

        public async Task<List<ToDoList>> GetAllToDoListsAsync()
        {
            List<ToDoList> allLists = new List<ToDoList>();
            foreach (var list in await _context.ToDoLists.ToListAsync())
            {
                if (_userService.CurrentUser.Id == list.CreatorId)
                {
                    allLists.Add(list);
                }
            }

            var sharedLists = await _context.SharedToDoLists.ToListAsync();
            foreach (var item in sharedLists)
            {
                if (_userService.CurrentUser.Id == item.UserId)
                {
                    var list = GetToDoListById(item.ListId);
                    allLists.Add(list);
                }
            }

            return allLists;
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

            var sharedLists = _context.SharedToDoLists.ToList();

            foreach (var item in sharedLists)
            {
                if (_userService.CurrentUser.Id == item.UserId)
                {
                    var list = GetToDoListById(item.ListId);

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

        public ToDoList GetToDoListById(int id)
        {
            return _applicationToDoLists.FirstOrDefault(u => u.Id == id);
        }
    }
}
