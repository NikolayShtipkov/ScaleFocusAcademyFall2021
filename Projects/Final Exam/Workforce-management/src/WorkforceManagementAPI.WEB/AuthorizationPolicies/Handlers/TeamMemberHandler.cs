using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL.Contracts.IdentityContracts;
using WorkforceManagementAPI.WEB.AuthorizationPolicies.Requirements;

namespace WorkforceManagementAPI.WEB.AuthorizationPolicies.Handlers
{
    public class TeamMemberHandler : AuthorizationHandler<TeamMemberRequirement>
    {
        private IIdentityUserManager userManager;

        public TeamMemberHandler(IIdentityUserManager userManager)
        {
            this.userManager = userManager;
        }

        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, TeamMemberRequirement requirement)
        {
            var loggedUser = await userManager.GetUserAsync(context.User);
            
            if (loggedUser != null && loggedUser.Teams.Count != 0)
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
    }
}
