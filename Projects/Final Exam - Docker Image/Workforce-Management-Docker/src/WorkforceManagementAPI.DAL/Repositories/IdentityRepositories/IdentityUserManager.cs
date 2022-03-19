using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DAL.Contracts.IdentityContracts;

namespace WorkforceManagementAPI.DAL.Repositories.IdentityRepositories
{
    public class IdentityUserManager : UserManager<User>, IIdentityUserManager
    {
        public IdentityUserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators, IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger)

            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {

        }

        public override Task<User> GetUserAsync(ClaimsPrincipal principal)
        {
            return base.GetUserAsync(principal);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await Users.ToListAsync();
        }

        public async Task<List<string>> GetUserRolesAsync(User user)
        {
            return (await GetRolesAsync(user)).ToList();
        }

        public async Task<IdentityResult> CreateUserAsync(User user, string password)
        {
            return await CreateAsync(user, password);
        }

        public async Task<IdentityResult> DeleteUserAsync(User user)
        {
            return await DeleteAsync(user);
        }

        public async Task<IdentityResult> UpdateUserDataAsync(User user)
        {
            return await UpdateAsync(user);
        }

        public async Task<bool> VerifyEmail(string email)
        {
            if (await Users.AnyAsync(u => u.Email == email))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> IsUserInRole(string userId, string roleName)
        {
            User user = await FindByIdAsync(userId);
            return await IsInRoleAsync(user, roleName);
        }

        public async Task<bool> ValidateUserCredentials(string userName, string password)
        {
            User user = await FindByNameAsync(userName);
            if (user != null)
            {
                bool result = await CheckPasswordAsync(user, password);
                return result;
            }
            return false;
        }

        public async Task<IdentityResult> AddUserToRoleAsync(User user, string password)
        {
            return await AddToRoleAsync(user, password);
        }
    }
}
