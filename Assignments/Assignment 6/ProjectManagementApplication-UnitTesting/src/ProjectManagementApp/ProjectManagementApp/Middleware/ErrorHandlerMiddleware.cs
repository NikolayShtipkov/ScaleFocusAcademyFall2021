using Microsoft.AspNetCore.Http;
using ProjectManagementApp.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectManagementApp.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case TeamNotFoundException teamNotFound:
                    case ProjectNotFoundException projectNotFound:
                    case TaskNotFoundException taskNotFound:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case UserExistsException userExistException:
                    case TeamExistsException teamExistException:
                    case ProjectExistsException projectExistException:
                    case TaskExistsException taskExistException:
                        response.StatusCode = (int)HttpStatusCode.Conflict;
                        break;
                    case UserNotFoundException userNotFound:
                    case UserUnauthorizedException unauthorizedUserException:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
