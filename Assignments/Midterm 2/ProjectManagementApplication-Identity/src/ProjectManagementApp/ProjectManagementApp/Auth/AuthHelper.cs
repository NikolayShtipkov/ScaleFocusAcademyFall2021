using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using System.Linq;
using Microsoft.AspNetCore.Http;
using ProjectManagementApp.Data.Entities;
using ProjectManagementApp.Services;
using ProjectManagementApp.Interfaces;

namespace ProjectManagementApp.Auth
{
    public static class AuthHelper
    {
        public static User GetCurrentUser(this IUserService userService, HttpRequest request)
        {
            StringValues UsernameAuthHeader;
            StringValues PasswordAuthHeader;

            request.Headers.TryGetValue("Username", out UsernameAuthHeader);
            request.Headers.TryGetValue("Password", out PasswordAuthHeader);

            if (UsernameAuthHeader.Count != 0 && PasswordAuthHeader.Count != 0)
            {
                string username = UsernameAuthHeader.First();
                string password = PasswordAuthHeader.First();

                return userService.LogIn(username, password);
            }

            return null;
        }
    }
}
