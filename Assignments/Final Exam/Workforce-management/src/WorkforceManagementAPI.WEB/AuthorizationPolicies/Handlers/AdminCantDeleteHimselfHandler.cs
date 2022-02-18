using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL.Contracts.IdentityContracts;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.WEB.AuthorizationPolicies.Requirements;

namespace WorkforceManagementAPI.WEB.AuthorizationPolicies.Handlers
{
    public class AdminCantDeleteHimselfHandler : AuthorizationHandler<AdminCantDeleteHimselfRequirement>
    {
        private IHttpContextAccessor httpContextAccessor;
        private IIdentityUserManager userManager;

        public AdminCantDeleteHimselfHandler(IHttpContextAccessor httpContextAccessor, IIdentityUserManager userManager)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminCantDeleteHimselfRequirement requirement)
        {
            var foundUser = await ValidateInputUserId();
            var loggedUser = await ValidateLoggedUserCredentials(context);

            if (foundUser != null && loggedUser != null && foundUser.Id != loggedUser.Id)
            {
                context.Succeed(requirement);
                await Task.CompletedTask;
            }
            else
            {
                context.Fail();
                await Task.CompletedTask;
            }
        }

        private async Task<User> ValidateInputUserId()
        {
            var userId = httpContextAccessor.HttpContext.GetRouteValue("userId").ToString();
            var foundUser = await userManager.FindByIdAsync(userId);

            if (foundUser != null)
            {
                return foundUser;
            }
            else
            {
                return null;
            }
        }

        private async Task<User> ValidateLoggedUserCredentials(AuthorizationHandlerContext context)
        {
            var foundUser = await userManager.GetUserAsync(context.User);

            if (foundUser != null)
            {
                return foundUser;
            }
            else
            {
                return null;
            }
        }
    }
}
