using AutoMapper;
using Nager.Date;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Contracts;
using WorkforceManagementAPI.Common;
using WorkforceManagementAPI.DAL.Contracts;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DAL.Entities.Enums;
using WorkforceManagementAPI.DTO.Models.Requests;
using WorkforceManagementAPI.DTO.Models.Responses;

namespace WorkforceManagementAPI.BLL.Services
{
    public class TimeOffService : ITimeOffService
    {
        private readonly IValidationService _validationService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly ITimeOffRepository _timeOffRepository;

        public TimeOffService(IValidationService validationService, IUserService userService, INotificationService notificationService, IMapper mapper, ITimeOffRepository timeOffRepository)
        {
            _validationService = validationService;
            _userService = userService;
            _notificationService = notificationService;
            _mapper = mapper;
            this._timeOffRepository = timeOffRepository;
        }

        public async Task<bool> CreateTimeOffAsync(TimeOffRequestDTO timeOffRequest, string creatorId)
        {
            _validationService.EnsureInputFitsBoundaries(((int)timeOffRequest.Type), 0, Enum.GetNames(typeof(RequestType)).Length - 1);
            _validationService.EnsureDateRangeIsValid(timeOffRequest.StartDate, timeOffRequest.EndDate);
            _validationService.EnsureTodayIsWorkingDay(DateTime.Now);

            var user = await _userService.GetUserByIdAsync(creatorId);
            _validationService.EnsureUserExist(user);

            var timeOff = _mapper.Map<TimeOff>(timeOffRequest);

            _validationService.EnsureTimeOffRequestsDoNotOverlap(user, timeOff);

            if (timeOffRequest.Type == RequestType.Paid)
            {
                CheckAvailableDaysOff(user, timeOff);
            }

            timeOff.Status = Status.Created;
            timeOff.CreatedAt = DateTime.Now;
            timeOff.ModifiedAt = DateTime.Now;
            timeOff.CreatorId = creatorId;
            timeOff.Creator = user;
            timeOff.ModifierId = creatorId;
            timeOff.Modifier = user;

            string subject = timeOff.Type.ToString() + " Time Off";
            string message;

            var teamLeadersOff = _timeOffRepository.GetTeamLeadersOutOfOffice(user);

            timeOff.Reviewers = user.Teams
                .Select(t => t.TeamLeader)
                .Except(teamLeadersOff)
                .ToList();

            await _timeOffRepository.CreateTimeOffAsync(timeOff);

            if (timeOff.Reviewers.Count == 0)
            {
                await FinalizeRequestFeedback(timeOff, "Your time off request has been approved.");
                await _timeOffRepository.SaveChangesAsync();

                return true;
            }

            if (timeOff.Type == RequestType.SickLeave)
            {
                message = string.Format(Constants.SickMessage, user.FirstName, user.LastName, timeOff.StartDate, timeOff.EndDate, timeOff.Reason);

                timeOff.Status = Status.Approved;

                await _timeOffRepository.SaveChangesAsync();
            }
            else
            {
                message = string.Format(Constants.RequestMessage, user.FirstName, user.LastName, timeOff.StartDate.Date, timeOff.EndDate.Date, timeOff.Type, timeOff.Reason, timeOff.Id);

                user.Teams.ForEach(t => t.TeamLeader.UnderReviewRequests.Add(timeOff));
                await _timeOffRepository.SaveChangesAsync();
            }

            await _notificationService.Send(timeOff.Reviewers, subject, message);

            return true;
        }

        public async Task<List<TimeOff>> GetAllAsync()
        {
            return await _timeOffRepository.GetAllAsync();
        }

        public async Task<List<TimeOff>> GetMyTimeOffs(string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            _validationService.EnsureUserExist(user);

            return await _timeOffRepository.GetMyTimeOffsAsync(userId);
        }

        public async Task<TimeOff> GetTimeOffAsync(Guid id)
        {
            return await _timeOffRepository.GetTimeOffAsync(id);
        }

        public async Task<bool> DeleteTimeOffAsync(Guid id)
        {
            TimeOff timeOff = await GetTimeOffAsync(id);

            _validationService.EnsureTimeOffExist(timeOff);
            _validationService.EnsureTimeOffRequestIsNotCompleted(timeOff);

            _timeOffRepository.DeleteTimeOffAsync(timeOff);
            await _timeOffRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditTimeOffAsync(Guid id, EditTimeOffRequestDTO timeoffRequest, User modifier)
        {

            _validationService.EnsureDateRangeIsValid(timeoffRequest.StartDate, timeoffRequest.EndDate);

            var timeOff = await GetTimeOffAsync(id);
            _validationService.EnsureTimeOffExist(timeOff);
            var dbTimeOff = await _timeOffRepository.GetTimeOffAsync(id);

            dbTimeOff.StartDate = timeoffRequest.StartDate;
            dbTimeOff.EndDate = timeoffRequest.EndDate; 

            _validationService.EnsureInputFitsBoundaries(((int)timeOff.Type), 0, Enum.GetNames(typeof(RequestType)).Length - 1);
            _validationService.EnsureTimeOffRequestsDoNotOverlap(modifier, dbTimeOff);
            _validationService.EnsureTimeOffRequestIsNotCompleted(timeOff);

            timeOff.Reason = timeoffRequest.Reason;
            timeOff.Type = timeOff.Type;
            timeOff.StartDate = timeoffRequest.StartDate;
            timeOff.EndDate = timeoffRequest.EndDate;
            timeOff.ModifiedAt = DateTime.Now;
            timeOff.ModifierId = modifier.Id;
            timeOff.Modifier = modifier;


            await _timeOffRepository.SaveChangesAsync();

            string message = string.Format(Constants.RequestMessage, modifier.FirstName, modifier.LastName, timeOff.StartDate.Date, timeOff.EndDate.Date, timeOff.Type, timeOff.Reason, timeOff.Id);

            await _notificationService.Send(timeOff.Reviewers, "Edited", message);

            return true;
        }

        public async Task<bool> SubmitFeedbackForTimeOffRequestAsync(User user, Guid timeOffId, Status status)
        {
            var timeOff = await GetTimeOffAsync(timeOffId);
            _validationService.EnsureTimeOffExist(timeOff);
            _validationService.EnsureNoReviewersLeft(timeOff);
            _validationService.EnsureUserIsReviewer(timeOff, user);
            _validationService.EnsureResponseIsValid(status);

            timeOff.Reviewers.Remove(user);
            user.UnderReviewRequests.Remove(timeOff);

            await _timeOffRepository.SaveChangesAsync();

            var message = UpdateRequestStatus(status, timeOff);
            await _timeOffRepository.SaveChangesAsync();

            bool allReviewersGaveFeedback = timeOff.Reviewers.Count == 0;
            if (allReviewersGaveFeedback)
            {
                if (timeOff.Type == RequestType.Paid)
                {
                    CheckAvailableDaysOff(user, timeOff);
                }
                await FinalizeRequestFeedback(timeOff, message);
            }

            await _timeOffRepository.SaveChangesAsync();

            return true;
        }

        private string UpdateRequestStatus(Status status, TimeOff timeOff)
        {
            if (status == Status.Rejected)
            {
                timeOff.Status = Status.Rejected;
                timeOff.Reviewers.Clear();
                return "Your time off request has been rejected.";
            }

            timeOff.Status = Status.Awaiting;

            return string.Empty;
        }

        private async Task FinalizeRequestFeedback(TimeOff timeOff, string message)
        {
            if (timeOff.Status != Status.Rejected)
            {
                message = "Your time off request has been approved.";
                timeOff.Status = Status.Approved;
            }

            await _notificationService.Send(new List<User>() { timeOff.Creator }, "response", message);
        }

        private void CheckAvailableDaysOff(User user, TimeOff timeOff)
        {
            var approvedTimeOffs = _timeOffRepository.GetApprovedTimeOffs(user);
            int totalDaysTaken = GetDaysTaken(approvedTimeOffs);
            int daysRequested = ((int)(timeOff.EndDate - timeOff.StartDate).TotalDays + 1) - GetHolidaysFromCurrentRequest(timeOff);

            _validationService.EnsureUserHasEnoughDays(totalDaysTaken, daysRequested);
        }

        public int GetDaysTaken(List<TimeOff> timeOffs)
        {
            int totalDaysTaken = timeOffs.Sum(t => (int)(t.EndDate - t.StartDate).TotalDays + 1);

            foreach (var timeOff in timeOffs)
            {
                totalDaysTaken -= GetHolidaysFromCurrentRequest(timeOff);
            }

            return totalDaysTaken;
        }

        public int GetHolidaysFromCurrentRequest(TimeOff timeOff)
        {
            int countHolidays = 0;

            for (DateTime curr = timeOff.StartDate; curr <= timeOff.EndDate; curr = curr.AddDays(1))
            {
                if (DateSystem.IsWeekend(curr, CountryCode.BG) || DateSystem.IsPublicHoliday(curr, CountryCode.BG))
                {
                    countHolidays++;
                }
            }

            return countHolidays;
        }

        public OffDaysDTO GetOffDays(User user)
        {
            var timeOffs = _timeOffRepository.GetApprovedTimeOffs(user);

            return new OffDaysDTO()
            {
                AllDays = 20,
                Remaining = 20 - GetDaysTaken(timeOffs),
                UsedDays = GetDaysTaken(timeOffs)
            };
        }
    }
}