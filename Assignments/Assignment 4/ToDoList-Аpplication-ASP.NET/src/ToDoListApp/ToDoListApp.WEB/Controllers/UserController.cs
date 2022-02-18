using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDoListApp.Data;
using ToDoListApp.Entities;
using ToDoListApp.Models.DTO.Responses;
using ToDoListApp.Models.DTO.Requests;
using ToDoListApp.Services;
using Microsoft.AspNetCore.Authorization;
using ToDoApp.Auth;
using Microsoft.Extensions.Configuration;

namespace ToDoListApp.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private static UserService _userService;

        public UserController(UserService userService) : base()
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetAllUsersAsync()
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            if (!currentUser.IsAdmin)
            {
                return Unauthorized("You must have administritive privilages to access user services");
            }

            List<User> users = await _userService.GetAllUsersAsync();
            List<UserResponseDTO> userResponseDTOs = new List<UserResponseDTO>();

            foreach (var user in users)
            {
                userResponseDTOs.Add(MapUser(user));
            }

            return userResponseDTOs;
        }

        [HttpGet("{id}")]
        public ActionResult<UserResponseDTO> GetUserById(int id)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            if (!currentUser.IsAdmin)
            {
                return Unauthorized("You must have administritive privilages to use this service");
            }

            var user =  _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound("Invalid input. User Id doesn't exist.");
            }

            return MapUser(user);
        }

        [HttpPost]
        public async Task<ActionResult> CreateUserAsync(UserCreateRequestDTO user)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            if (!currentUser.IsAdmin)
            {
                return Unauthorized("You must have administritive privilages to use this service");
            }

            if (ModelState.IsValid)
            {
                bool isCreated = await _userService.CreateUserAsync(user.Username, user.Password, user.FirstName, user.LastName, user.IsAdmin);
                if (isCreated)
                {
                    return Ok("User created successfully.");
                }
            }

            return BadRequest("User couldn't be created, please check input.");
        }

        [HttpPut("{id}")]
        public ActionResult EditUser(int id, UserEditRequestDTO userEdit)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            if (!currentUser.IsAdmin)
            {
                return Unauthorized("You must have administritive privilages to use this service");
            }

            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound("Invalid input. User Id doesn't exist.");
            }

            bool isEmpty = userEdit.Username == null || userEdit.Password == null || userEdit.FirstName == null || userEdit.LastName == null;
            if (isEmpty)
            {
                return BadRequest("Can't have empty fields");
            }

            bool isEdited = _userService.EditUser(id, userEdit.Username, userEdit.Password, userEdit.FirstName, userEdit.LastName);
            if (!isEdited)
            {
                return BadRequest("Username already in use.");
            }

            return Ok($"User with Id: {id} was edited successfully");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (currentUser == null)
            {
                return BadRequest("Invalid input. Incorrect username or password.");
            }

            if (!currentUser.IsAdmin)
            {
                return Unauthorized("You must have administritive privilages to use this service");
            }

            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound("Invalid input. User Id doesn't exist.");
            }

            bool isDeleted = _userService.DeleteUser(id);
            if (!isDeleted)
            {
                return BadRequest("User can't be deleted, please check input");
            }

            return Ok($"User with Id: {id} was deleted successfully");
        }

        private UserResponseDTO MapUser(User userEntity)
        {
            var user = new UserResponseDTO()
            {
                Id = userEntity.Id,
                Username = userEntity.Username,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                isAdmin = userEntity.IsAdmin,
                CreatorId = userEntity.CreatorId,
                ModifierId = userEntity.ModifierId,
                CreatedAt = userEntity.CreatedAt,
                ModifiedAt = userEntity.ModifiedAt
            };

            return user;
        }
    }
}
