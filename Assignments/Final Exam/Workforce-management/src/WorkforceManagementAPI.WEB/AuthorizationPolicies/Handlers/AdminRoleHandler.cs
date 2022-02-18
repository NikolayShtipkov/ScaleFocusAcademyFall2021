using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using WorkforceManagementAPI.WEB.AuthorizationPolicies.Requirements;

namespace WorkforceManagementAPI.WEB.AuthorizationPolicies.Handlers
{
    public class AdminRoleHandler : AuthorizationHandler<TimeOffCreatorOrAdminRequirement>
    {
        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, TimeOffCreatorOrAdminRequirement requirement)
        {
            if (context.User != null && context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
                await Task.CompletedTask;
            }
        }
    }
}
