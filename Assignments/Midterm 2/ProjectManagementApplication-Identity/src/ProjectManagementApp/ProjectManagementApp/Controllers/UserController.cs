using Microsoft.AspNetCore.Mvc;
using ProjectManagementApp.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using ProjectManagementApp.Services;
using ProjectManagementApp.DTO.Requests;
using ProjectManagementApp.DTO.Responses;
using ProjectManagementApp.Data.Entities;
using ProjectManagementApp.Auth;
using ProjectManagementApp.Interfaces;

namespace ProjectManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private static IUserService _userService;
        private static ITeamService _teamService;

        public UserController(IUserService userService, ITeamService teamService) : base()
        {
            _userService = userService;
            _teamService = teamService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetAllUsersAsync()
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (!currentUser.IsAdmin)
            {
                return Unauthorized("You must have administritive privilages to access user services.");
            }

            var users = await _userService.GetAllUsersAsync();
            var userResponseDTOs = new List<UserResponseDTO>();

            foreach (var user in users)
            {
                userResponseDTOs.Add(MapUser(user));
            }

            return userResponseDTOs;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDTO>> GetUserByIdAsync(int id)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (!currentUser.IsAdmin)
            {
                return Unauthorized("You must have administritive privilages to use this service.");
            }

            var user = await _userService.GetUserByIdAsync(id);

            return MapUser(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(UserCreateRequestDTO user)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (!currentUser.IsAdmin)
            {
                return Unauthorized("You must have administritive privilages to use this service.");
            }

            bool isCreated = await _userService.CreateUserAsync(user.Username, user.Password, user.FirstName, user.LastName, user.IsAdmin, user.TeamId);
            if (isCreated && ModelState.IsValid)
            {
                return Ok("User created successfully.");
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditUserAsync(int id, UserEditRequestDTO userEdit)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (!currentUser.IsAdmin)
            {
                return Unauthorized("You must have administritive privilages to use this service.");
            }

            bool isEdited = await _userService.EditUserAsync(id, userEdit.Username, userEdit.Password, userEdit.FirstName, userEdit.LastName);
            if (isEdited)
            {
                return Ok($"User with Id: {id} was edited successfully.");
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            var currentUser = _userService.GetCurrentUser(Request);
            if (!currentUser.IsAdmin)
            {
                return Unauthorized("You must have administritive privilages to use this service.");
            }

            bool isDeleted = await _userService.DeleteUserAsync(id);
            if (isDeleted)
            {
                return Ok($"User with Id: {id} was deleted successfully.");
            }

            return BadRequest();
        }

        private UserResponseDTO MapUser(User userEntity)
        {
            var user = new UserResponseDTO()
            {
                Id = userEntity.Id,
                Username = userEntity.Username,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                IsAdmin = userEntity.IsAdmin
            };

            return user;
        }
    }
}
