using ToDoListApp.Models;
using ToDoListApp.Entities;
using ToDoListApp.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ToDoListApp.Services;
using Microsoft.Extensions.Primitives;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace ToDoApp.Auth
{
    public static class AuthHelper
    {
        public static User GetCurrentUser(this UserService userService, HttpRequest request)
        {
            StringValues UsernameAuthHeader;
            StringValues PasswordAuthHeader;

            request.Headers.TryGetValue("Username", out UsernameAuthHeader);
            request.Headers.TryGetValue("Password", out PasswordAuthHeader);

            if (UsernameAuthHeader.Count != 0 && PasswordAuthHeader.Count != 0)
            {
                string username = UsernameAuthHeader.First();
                string password = PasswordAuthHeader.First();

                return userService.Login(username, password);
            }

            return null;
        }
    }
}
