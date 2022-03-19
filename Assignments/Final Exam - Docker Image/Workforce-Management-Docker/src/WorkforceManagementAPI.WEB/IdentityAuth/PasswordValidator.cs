using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL.Contracts.IdentityContracts;
using WorkforceManagementAPI.DAL.Entities;

namespace WorkforceManagementAPI.WEB.IdentityAuth
{
    public class PasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IIdentityUserManager userManager;

        public PasswordValidator(IIdentityUserManager userManager)
        {
            this.userManager = userManager;
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {

           User user = await userManager.FindByNameAsync(context.UserName);

            if (user != null)
            {
                bool authResult = await userManager.ValidateUserCredentials(context.UserName, context.Password);

                if (authResult)
                {
                    List<string> roles = await userManager.GetUserRolesAsync(user);
                    List<Claim> claims = new List<Claim>();

                    claims.Add(new Claim(ClaimTypes.Name, user.UserName));

                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    context.Result = new GrantValidationResult(subject: user.Id, authenticationMethod: "password", claims: claims);
                }
                else
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid login credentials");
                }

                return;
            }
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid login credentials");
        }
    }
}
