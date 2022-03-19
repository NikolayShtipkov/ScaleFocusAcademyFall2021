using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Auth;
using ToDoListApp.Data;
using ToDoListApp.Entities;
using ToDoListApp.Models.DTO.Requests;
using ToDoListApp.Models.DTO.Responses;
using ToDoListApp.Services;

namespace ToDoListApp.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoListController : ControllerBase
    {
        private static UserService _userService;
        private static ToDoListService _toDoListService;

        public ToDoListController(UserService userService, ToDoListService toDoListService) : base()
        {
            _userService = userService;
            _toDoListService = toDoListService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoListResponseDTO>>> GetAllToDoListsAsync()
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            var toDoLists = await _toDoListService.GetAllToDoListsAsync();
            if (toDoLists.Count == 0)
            {
                return Ok("No created or shared ToDo lists with this user.");
            }

            var responseToDoLists = new List<ToDoListResponseDTO>();
            foreach (var list in toDoLists)
            {
                responseToDoLists.Add(MapToDoList(list));
            }

            return responseToDoLists;
        }

        [HttpGet("{id}")]
        public ActionResult<ToDoListResponseDTO> GetToDoListById(int id)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            var toDoList = _toDoListService.GetToDoListById(id);
            if (toDoList == null)
            {
                return NotFound("Invalid input. ToDoList Id doesn't exist.");
            }

            bool hasAccess = _toDoListService.IsToDoListShared(currentUser.Id, toDoList.Id) || currentUser.Id == toDoList.CreatorId;
            if (!hasAccess)
            {
                return Unauthorized("You don't have accsses to this ToDoList.");
            }

            return MapToDoList(toDoList); ;
        }

        [HttpPost]
        public ActionResult CreateToDoList(ToDoListRequestDTO toDoList)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            if (ModelState.IsValid)
            {
                bool isCreated =_toDoListService.CreateToDoList(toDoList.Title);
                if (isCreated)
                {
                    return Ok("ToDoList created successfully.");
                }
            }
            
            return BadRequest("Title is already in use");
        }

        [HttpPut("{id}")]
        public ActionResult EditToDoList(int id, ToDoListRequestDTO toDoList)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            var list = _toDoListService.GetToDoListById(id);
            if (list == null)
            {
                return NotFound("Invalid input. ToDoList Id doesn't exist.");
            }

            bool hasAccess = _toDoListService.IsToDoListShared(currentUser.Id, list.Id) || currentUser.Id == list.CreatorId;
            if (!hasAccess)
            {
                return StatusCode(403);
            }

            bool isEdited = _toDoListService.EditToDoList(id, toDoList.Title);
            if (!isEdited)
            {
                return Unauthorized("Title already in use");
            }

            return Ok($"ToDoList with Id: {id} was edited successfully");
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteToDoList(int id)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            var toDoList = _toDoListService.GetToDoListById(id);
            if (toDoList == null)
            {
                return NotFound("Invalid input. ToDoList Id doesn't exist.");
            }

            bool hasAccess = _toDoListService.IsToDoListShared(currentUser.Id, toDoList.Id) || currentUser.Id == toDoList.CreatorId;
            if (!hasAccess)
            {
                return StatusCode(403);
            }

            bool isDeleted = _toDoListService.DeleteToDoLIst(id);
            if (!isDeleted)
            {
                return Unauthorized("ToDoList can't be delted");
            }

            if (currentUser.Id != toDoList.CreatorId)
            {
                return Ok($"Share of ToDoList with Id: {id} was deleted successfully");
            }

            return Ok($"ToDoList with Id: {id} was deleted successfully");
        }

        [HttpPost("{userId}/Share/{toDoListId}")]
        public ActionResult ShareToDoList(int userId, int toDoListId)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            var user = _userService.GetUserById(userId);
            if (user == null)
            {
                return NotFound("Invalid input. User Id doesn't exist.");
            }

            var toDoList = _toDoListService.GetToDoListById(toDoListId);
            if (toDoList == null)
            {
                return NotFound("Invalid input. ToDoList Id doesn't exist.");
            }

            bool hasShare = _toDoListService.IsToDoListShared(userId, toDoList.Id);
            if (hasShare)
            {
                return BadRequest("ToDo list has already been shared with user.");
            }

            bool isCreator = currentUser.Id == userId;
            if (isCreator)
            {
                return Ok("User is creator of the list and already has access.");
            }

            bool isShared = _toDoListService.ShareToDoList(userId, toDoListId);
            if (!isShared)
            {
                return StatusCode(403);
            }

            return Ok($"Successfully shared list with Id: {toDoListId} to user with Id: {userId}.");
        }

        private ToDoListResponseDTO MapToDoList(ToDoList list)
        {
            var toDoList = new ToDoListResponseDTO()
            {
                Id = list.Id,
                Title = list.Title,
                CreatorId = list.CreatorId,
                ModifierId = list.ModifierId,
                CreatedAt = list.CreatedAt,
                ModifiedAt = list.ModifiedAt
            };

            return toDoList;
        }
    }
}
