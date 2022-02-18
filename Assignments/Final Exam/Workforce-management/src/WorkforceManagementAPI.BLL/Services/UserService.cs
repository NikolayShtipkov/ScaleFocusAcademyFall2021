using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Contracts;
using WorkforceManagementAPI.DAL.Contracts.IdentityContracts;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DTO.Models.Requests;

namespace WorkforceManagementAPI.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IIdentityUserManager _userManager;
        private readonly IValidationService _validationService;
        private readonly IMapper _mapper;

        public UserService(IIdentityUserManager userManager, IValidationService validationService, IMapper mapper)
        {
            _userManager = userManager;
            _validationService = validationService;
            _mapper = mapper;
        }

        public async Task<bool> CreateUserAsync(CreateUserRequestDTO userRequest)
        {
            _validationService.EnsureLenghtIsValid(userRequest.Password, 7, nameof(userRequest.Password));
            _validationService.EnsureLenghtIsValid(userRequest.FirstName, 2, nameof(userRequest.FirstName));
            _validationService.EnsureLenghtIsValid(userRequest.LastName, 2, nameof(userRequest.LastName));

            _validationService.EnsureEmailIsValid(userRequest.Email);
            await _validationService.EnsureEmailIsUniqueAsync(userRequest.Email);

            var user = _mapper.Map<User>(userRequest);
            user.UserName = userRequest.Email;
            await _userManager.CreateUserAsync(user, userRequest.Password);
           
            return true;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var checkUser = await _userManager.FindByIdAsync(id);
            _validationService.EnsureUserExist(checkUser);
            _validationService.EnsureUserIsNotInTeam(checkUser);
            _validationService.EnsureUserDoesntHaveRequests(checkUser);
            
            await _userManager.DeleteUserAsync(checkUser);
            return true;
        }

        public async Task<bool> UpdateUserAsync(string userId, EditUserRequest editUserReaqest)
        {
            _validationService.EnsureLenghtIsValid(editUserReaqest.NewPassword, 7, nameof(editUserReaqest.NewPassword));
            _validationService.EnsureLenghtIsValid(editUserReaqest.NewFirstName, 2, nameof(editUserReaqest.NewFirstName));
            _validationService.EnsureLenghtIsValid(editUserReaqest.NewLastName, 2, nameof(editUserReaqest.NewLastName));

            User user = await _userManager.FindByIdAsync(userId);
            _validationService.EnsureUserExist(user);

            _validationService.EnsureEmailIsValid(editUserReaqest.NewEmail);
            await _validationService.EnsureUpdateEmailIsUniqueAsync(editUserReaqest.NewEmail, user);

            PasswordHasher<User> hasher = new PasswordHasher<User>();
            user.UserName = editUserReaqest.NewEmail;
            user.FirstName = editUserReaqest.NewFirstName;
            user.LastName = editUserReaqest.NewLastName;
            user.PasswordHash = hasher.HashPassword(user, editUserReaqest.NewPassword);
            user.Email = editUserReaqest.NewEmail;
            await _userManager.UpdateUserDataAsync(user);

            return true;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userManager.GetAllAsync();
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            var checkUser = await _userManager.FindByIdAsync(id);
            _validationService.EnsureUserExist(checkUser);

            return checkUser;
        }

        public async Task<User> GetCurrentUser(ClaimsPrincipal principal)
        {
            return await _userManager.GetUserAsync(principal);
        }

        public async Task SetAdministratorAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            _validationService.EnsureUserExist(user);
            await _validationService.EnsureUserIsAdminAsync(user);
            await _userManager.AddUserToRoleAsync(user, "Admin");
        }
    }
}
