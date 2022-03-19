using System;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DAL.Entities.Enums;

namespace WorkforceManagementAPI.BLL.Contracts
{
    public interface IValidationService
    {
        void EnsureUserExist(User user);

        void EnsureLenghtIsValid(string forCheck, int length, string message);

        void EnsureEmailIsValid(string email);

        Task EnsureEmailIsUniqueAsync(string email);

        void EnsureTeamExist(Team team);

        void EnsureTeamNameIsUniquee(string title);

        void EnsureTimeOffExist(TimeOff timeOff);

        Task EnsureUpdateEmailIsUniqueAsync(string email, User user);

        void EnsureInputFitsBoundaries(int input, int minValue, int maxValue);

        void EnsureDateRangeIsValid(DateTime minValue, DateTime maxValue);

        void EnsureInputFitsBoundaries(DateTime input, DateTime minValue, DateTime maxValue);

        void EnsureTeamNameIsUniqueWhenEdit(string newTitle, string oldTitle);

        void EnsureUserHasAccessToTeam(Team team, User user);

        void EnsureUserIsNotAlreadyATeamLeader(Team team, User user);

        void EnsureUserIsNotAlreadyPartOfTheTeam(Team team, User user);
        void EnsureNoReviewersLeft(TimeOff timeOff);
        void EnsureUserIsReviewer(TimeOff timeOff, User user);
        void EnsureResponseIsValid(Status status);
        Task EnsureUserIsAdminAsync(User user);
        void EnsureUserHasEnoughDays(int daysTaken, int daysRequested);
        void EnsureTimeOfRequestsDoNotOverlap(User user, TimeOff timeOff);
        void EnsureTodayIsWorkingDay();
        void EnsureUserIsNotInTeam(User user);
        void EnsureUnassignUserHasAccessToTeam(Team team, User user);
    }
}
