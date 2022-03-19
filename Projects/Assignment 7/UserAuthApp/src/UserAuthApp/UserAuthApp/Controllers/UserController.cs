using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAuthApp.BLL.Interfaces;
using UserAuthApp.DAL.Entities;
using UserAuthApp.Models.DTO.Requests;
using UserAuthApp.Models.DTO.Responses;

namespace UserAuthApp.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
            : base()
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserRequestDTO user)
        {

            User currentUser = await _userService.GetCurrentUser(User);

            // This is not needed
            if (await _userService.IsUserInRole(currentUser.Id, "Admin"))
            {
                bool result = await _userService.CreateUser(user.UserName, user.Password);

                if (result)
                {
                    User userFromDB = await _userService.GetUserByName(user.UserName);

                    return CreatedAtAction("Get", "User", new { id = userFromDB.Id }, null);
                }
                else
                {
                    return BadRequest();
                }
            }
            return Unauthorized();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<UserResponseDTO> Get(string id)
        {
            User userFromDB = await _userService.GetUserById(id);
            return new UserResponseDTO()
            {
                UserName = userFromDB.UserName,
                Id = userFromDB.Id,
            };
        }

        [HttpGet]
        public async Task<List<UserResponseDTO>> GetAll()
        {
            List<UserResponseDTO> users = new List<UserResponseDTO>();

            foreach (var user in await _userService.GetAll())
            {
                users.Add(new UserResponseDTO()
                {
                    Id = user.Id,
                    UserName = user.UserName
                });
            }

            return users;
        }
    }
}
