using Microsoft.AspNetCore.Authorization;

namespace WorkforceManagementAPI.WEB.AuthorizationPolicies.Requirements
{
    public class TimeOffCreatorOrAdminRequirement : IAuthorizationRequirement
    {
    }
}
