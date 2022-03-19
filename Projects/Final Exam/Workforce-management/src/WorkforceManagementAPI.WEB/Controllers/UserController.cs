using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Services;
using WorkforceManagementAPI.Common;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DTO.Models.Requests;
using WorkforceManagementAPI.DTO.Models.Responses;

namespace WorkforceManagementAPI.WEB.Controllers
{
   
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService,IMapper mapper) : base()
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Create a user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <response code="201">Created - Request resulted in new resource created.</response>
        /// <response code="400">BadRequest - Request could not be understood by the server.</response>
        /// <response code="401">Unauthorized - Please check the provided credentials.</response>
        /// <response code="403">Forbidden - Your credentials don't meet the required authorization level to access the resource. 
        ///Please, contact your administrator to get desired permissions.</response>
        /// <response code="404">NotFound - Requested information does not exist in the server.</response>
        /// <response code="409">Conflict - The submitted entity ran into a conflict with an existing one.</response>
        /// <response code="500">InternalServerError - Generic error occured in the server.</response>
        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(CreateUserRequestDTO user)
        {
            bool result = await _userService.CreateUserAsync(user);
            if (result)
            {
                return Created(nameof(HttpPostAttribute), String.Format(Constants.Created, "User"));
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// List all users, existing in the database.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">OK - Request succeeded.</response>
        /// <response code="401">Unauthorized - Please check the provided credentials.</response>
        /// <response code="403">Forbidden - Your credentials don't meet the required authorization level to access the resource. 
        ///Please, contact your administrator to get desired permissions.</response>
        [HttpGet]
        public async Task<List<UserResponseDTO>> GetAllUsersAsync()
        {
            var users = await _userService.GetAllUsersAsync();
            return _mapper.Map<List<UserResponseDTO>>(users);
        }

        /// <summary>
        /// Find a user by user Id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <response code="200">OK - Request succeeded.</response>
        /// <response code="400">BadRequest - Request could not be understood by the server.</response>
        /// <response code="401">Unauthorized - Please check the provided credentials.</response>
        /// <response code="403">Forbidden - Your credentials don't meet the required authorization level to access the resource. 
        ///Please, contact your administrator to get desired permissions.</response>
        /// <response code="404">NotFound - Requested information does not exist in the server.</response>
        /// <response code="500">InternalServerError - Generic error occured in the server.</response>
        [HttpGet("{userId}")]
        public async Task<UserResponseDTO> GetUserByIdAsync(Guid userId)
        {
            User user = await _userService.GetUserByIdAsync(userId.ToString());
            return _mapper.Map<UserResponseDTO>(user);
        }

        /// <summary>
        /// Edit a user by user Id.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <response code="200">OK - Request succeeded.</response>
        /// <response code="400">BadRequest - Request could not be understood by the server.</response>
        /// <response code="401">Unauthorized - Please check the provided credentials.</response>
        /// <response code="403">Forbidden - Your credentials don't meet the required authorization level to access the resource. 
        ///Please, contact your administrator to get desired permissions.</response>
        /// <response code="404">NotFound - Requested information does not exist in the server.</response>
        /// <response code="409">Conflict - The submitted entity ran into a conflict with an existing one.</response>
        /// <response code="500">InternalServerError - Generic error occured in the server.</response>
        [HttpPut("{userId}")]
        public async Task<IActionResult> EditUserAsync(Guid userId, EditUserRequest user)
        {
            bool isEdited = await _userService.UpdateUserAsync(userId.ToString(), user);
            if (isEdited && ModelState.IsValid)
            {
                return Ok("User edited successfully");
            }
            return BadRequest();
        }

        /// <summary>
        /// REMOVE a user by user Id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <response code="200">OK - Request succeeded.</response>
        /// <response code="400">BadRequest - Request could not be understood by the server.</response>
        /// <response code="401">Unauthorized - Please check the provided credentials.</response>
        /// <response code="403">Forbidden - Your credentials don't meet the required authorization level to access the resource. 
        ///Please, contact your administrator to get desired permissions.</response>
        /// <response code="404">NotFound - Requested information does not exist in the server.</response>
        /// <response code="409">Conflict - The submitted entity ran into a conflict with an existing one.</response>
        /// <response code="500">InternalServerError - Generic error occured in the server.</response>
        [Authorize(Policy = "AdminCantDeleteHimself")]  
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUserAsync(Guid userId)
        {
            bool isDeleted = await _userService.DeleteUserAsync(userId.ToString());
            if (isDeleted)
            {
                return Ok("User deleted successfully");
            }
            return BadRequest();
        }

        /// <summary>
        /// Set user as ADMINISTRATOR, using user Id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <response code="200">OK - Request succeeded.</response>
        /// <response code="400">BadRequest - Request could not be understood by the server.</response>
        /// <response code="401">Unauthorized - Please check the provided credentials.</response>
        /// <response code="403">Forbidden - Your credentials don't meet the required authorization level to access the resource. 
        ///Please, contact your administrator to get desired permissions.</response>
        /// <response code="404">NotFound - Requested information does not exist in the server.</response>
        /// <response code="409">Conflict - The submitted entity ran into a conflict with an existing one.</response>
        /// <response code="500">InternalServerError - Generic error occured in the server.</response>
        [HttpPost("SetAdmin/{userId}")]
        public async Task<IActionResult> SetAdministratorAsync(Guid userId)
        {
            await _userService.SetAdministratorAsync(userId.ToString());
            return Ok($"User: {userId} is set as Admin");
        }
    }
}
