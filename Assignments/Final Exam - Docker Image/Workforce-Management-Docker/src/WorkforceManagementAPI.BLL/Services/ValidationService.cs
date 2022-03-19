using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Contracts;
using WorkforceManagementAPI.BLL.Exceptions;
using WorkforceManagementAPI.Common;
using WorkforceManagementAPI.DAL;
using WorkforceManagementAPI.DAL.Contracts.IdentityContracts;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DAL.Entities.Enums;
using Nager.Date;

namespace WorkforceManagementAPI.BLL.Services
{
    public class ValidationService : IValidationService
    {
        private readonly DatabaseContext _context;
        private readonly IIdentityUserManager _userManager;

        public ValidationService(DatabaseContext context, IIdentityUserManager userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void EnsureUserExist(User user)
        {
            if (user == null)
            {
                throw new EntityNotFoundException(String.Format(Constants.NotFound, "User"));
            }
        }

        public void EnsureLenghtIsValid(string forCheck, int length, string message)
        {
            if (forCheck.Length <= length)
            {
                throw new InvalidLengthException(String.Format(Constants.InvalidLength, message, length));
            }
        }

        public void EnsureEmailIsValid(string email)
        {
            if (!new EmailAddressAttribute().IsValid(email) || email.Length <= 4)
            {
                throw new InvalidEmailException(Constants.InvalidEmail);
            }
        }

        public async Task EnsureEmailIsUniqueAsync(string email)
        {
            if (await _userManager.VerifyEmail(email) == false)
            {
                throw new EmailAlreadyInUseException(Constants.EmailAreadyInUse);
            }
        }

        public void EnsureTeamExist(Team team)
        {
            if (team == null)
            {
                throw new EntityNotFoundException(String.Format(Constants.NotFound, "Team"));
            }
        }

        public void EnsureTimeOffExist(TimeOff timeOff)
        {
            if (timeOff == null)
            {
                throw new EntityNotFoundException(String.Format(Constants.NotFound, "TimeOff"));
            }
        }

        public void EnsureTeamNameIsUniquee(string title)
        {
            if (_context.Teams.Any(p => p.Title == title))
            {
                throw new NameExistException(String.Format(Constants.NameAlreadyInUse, "Team name"));
            }
        }

        public void EnsureTeamNameIsUniqueWhenEdit(string newTitle, string oldTitle)
        {
            if (_context.Teams.Any(p => p.Title == newTitle) && newTitle != oldTitle)
            {
                throw new NameExistException(String.Format(Constants.NameAlreadyInUse, "Team name"));
            }
        }

        public async Task EnsureUpdateEmailIsUniqueAsync(string email,User user)
        {
            if (await _userManager.VerifyEmail(email) == false && user.Email != email)
            {
                throw new EmailAlreadyInUseException(Constants.EmailAreadyInUse);
            }
        }

        public void EnsureInputFitsBoundaries(int input, int minValue = 0, int maxValue = 1)
        {
            if(input < minValue || input > maxValue)
            {
                throw new InputOutOfBoundsException(String.Format(Constants.InputOutOfBounds, nameof(Int32)));

            }
        }

        public void EnsureDateRangeIsValid(DateTime startDate, DateTime endDate)
        {
            EnsureInputFitsBoundaries(startDate, DateTime.Now, new DateTime(DateTime.Now.Year + 1, 1, 1));
            EnsureInputFitsBoundaries(endDate, DateTime.Now, new DateTime(DateTime.Now.Year + 1, 1, 1));

            if (startDate > endDate)
            {
                throw new ArgumentException(Constants.InvalidInput);
            }
        }

        public void EnsureInputFitsBoundaries(DateTime input, DateTime minValue, DateTime maxValue)
        {
            if (input < minValue || input > maxValue)
            {
                throw new InputOutOfBoundsException(String.Format(Constants.InputOutOfBounds, nameof(DateTime)));
            }
        }

        public void EnsureUserHasAccessToTeam(Team team, User user)
        {
            if (!team.Users.Any(u => u.Id == user.Id))
            {
                throw new UnauthorizedUserException(Constants.TeamAccess);
            }
        }

        public void EnsureUnassignUserHasAccessToTeam(Team team, User user)
        {
            if (!team.Users.Any(u => u.Id == user.Id))
            {
                throw new UnauthorizedUserException("User is not part of the team");
            }
        }

        public void EnsureUserIsNotAlreadyATeamLeader(Team team, User user)
        {
            if (team.TeamLeaderId == user.Id)
            {
                throw new UserAlreadyTeamLeaderException(Constants.InvalidTeamLeader);
            }
        }

        public void EnsureUserIsNotAlreadyPartOfTheTeam(Team team, User user)
        {
            if (team.Users.Any(u => u.Id == user.Id))
            {
                throw new UserAlreadyInTeamException(Constants.UserAlreadyMember);
            }
        }

        public void EnsureNoReviewersLeft(TimeOff timeOff)
        {
            if (timeOff.Reviewers.Count == 0)
            {
                throw new CompletedRequestException(Constants.CompletedRequest);
            }
        }

        public void EnsureUserIsReviewer(TimeOff timeOff, User user)
        {
            if (!timeOff.Reviewers.Any(u => u.Id == user.Id))
            {
                throw new UnauthorizedUserException(Constants.NotReviewer);
            }
        }

        public void EnsureResponseIsValid(Status status)
        {
            if (status != Status.Rejected && status != Status.Approved)
            {
                throw new InputOutOfBoundsException(Constants.InvalidStatus);
            }
        }

        public async Task EnsureUserIsAdminAsync(User user)
        {
            if (await _userManager.IsUserInRole(user.Id, "Admin"))
            {
                throw new UserAlreadyAnAdminException(Constants.UserIsAdmin);
            }
        }

        public void EnsureUserHasEnoughDays(int daysTaken, int daysRequested)
        {
            if ((daysTaken + daysRequested) > 20)
            {
                throw new NotEnoughDaysForTimeOffException($"Days requested exceed yearly limit. You have {20 - daysTaken} days remaining from 20.");
            }
        }

        public void EnsureTimeOfRequestsDoNotOverlap(User user, TimeOff timeOff)
        {
            if (user.Requests.Any(r => r.Status != Status.Rejected && (r.StartDate.Date <= timeOff.EndDate.Date && timeOff.StartDate.Date <= r.EndDate.Date)))
            {
                throw new TimeOffOverlapException("You can't have TimeOff requests with overlaping start or end dates.");
            }
        }

        public void EnsureTodayIsWorkingDay()
        {
            var currrentDay = DateTime.Now;
            if (DateSystem.IsWeekend(currrentDay, CountryCode.BG) || DateSystem.IsPublicHoliday(currrentDay, CountryCode.BG))
            {
                throw new NotAWorkingDayException("Today is not a working day");
            }
        }

        public void EnsureUserIsNotInTeam(User user)
        {
            if (user.Teams.Count >= 1)
            {
                throw new UserIsInTeamException("User is part of a team and can't be deleted");
            }
        }


    }
}
