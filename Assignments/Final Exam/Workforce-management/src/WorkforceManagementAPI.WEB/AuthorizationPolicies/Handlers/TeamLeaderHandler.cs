using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Contracts;
using WorkforceManagementAPI.BLL.Services;
using WorkforceManagementAPI.DAL.Contracts.IdentityContracts;
using WorkforceManagementAPI.WEB.AuthorizationPolicies.Requirements;

namespace WorkforceManagementAPI.WEB.AuthorizationPolicies.Handlers
{
    public class TeamLeaderHandler : AuthorizationHandler<TeamLeaderRequirement>
    {
        private IIdentityUserManager userManager;
        private ITimeOffService timeOffService;
        private IHttpContextAccessor httpContextAccessor;
        private IUserService userService;

        public TeamLeaderHandler(IIdentityUserManager userManager, ITimeOffService timeOffService, IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            this.userManager = userManager;
            this.timeOffService = timeOffService;
            this.httpContextAccessor = httpContextAccessor;
            this.userService = userService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TeamLeaderRequirement requirement)
        {
            if (await TeamLeaderValidaition(context))
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

        private async Task<bool> TeamLeaderValidaition(AuthorizationHandlerContext context)
        {
            var loggedUser = await userManager.GetUserAsync(context.User);

            if (loggedUser != null)
            {
                var timeOffId = httpContextAccessor.HttpContext.GetRouteValue("timeOffId").ToString();
                Guid actualId = new Guid(timeOffId);
                var timeOff = await timeOffService.GetTimeOffAsync(actualId);

                if (timeOff != null)
                {
                    var timeOffCreator = await userService.GetUserByIdAsync(timeOff.CreatorId);
                    var isLoggedUserValidTeamLeader = timeOffCreator.Teams.Any(t => t.TeamLeaderId == loggedUser.Id);

                    return isLoggedUserValidTeamLeader;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}
