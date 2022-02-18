using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Exceptions;

namespace WebApi.Middleware
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
                    case EntityNotFoundException entity:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case NameExistException nameExistException:
                    case EmailAlreadyInUseException emailAlreadyInUse:
                    case UserAlreadyInTeamException userAlreadyInTeamException:
                    case UserAlreadyAnAdminException userAlreadyAnAdmin:
                    case UserAlreadyTeamLeaderException userAlreadyTeamLeaderException:
                    case TimeOffOverlapException timeOffAlreadyExists:
                    case UserIsInTeamException userIsInTeamException:
                    case UnauthorizedUserException unauthorizedUserException:
                    case TimeOffCompletedException timeOffCompletedException:
                        response.StatusCode = (int)HttpStatusCode.Conflict;
                        break;
                    case NotAWorkingDayException notAWorkingDay:
                    case InputOutOfBoundsException inputException:
                    case InvalidEmailException invalidEmail:
                    case InvalidLengthException invalidLength:
                    case UnauthorizedAccessException unauthorizedAccessException:
                    case NotEnoughDaysForTimeOffException notEnoughDaysException:
                    case RequestAlreadyCompletedException requestCompleted:
                    case ArgumentException argumentException:
                    case UserHasExistingRequestsException userHasExistingRequests: 
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
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

