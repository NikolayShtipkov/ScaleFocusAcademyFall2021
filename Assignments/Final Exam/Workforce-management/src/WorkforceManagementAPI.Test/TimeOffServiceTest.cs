using System;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DAL.Entities.Enums;
using WorkforceManagementAPI.DTO.Models.Requests;
using Xunit;
using System.Collections.Generic;
using WorkforceManagementAPI.DTO.Models.Responses;

namespace WorkforceManagementAPI.Test
{
    public class TimeOffServiceTest : ServicesTestBase
    {
        [Fact]
        public async Task Create_TimeOff_Successfully_ReturnsTrue()
        {

            var timeOffService = SetupMockedTimeOffService();
            TimeOffRequestDTO newTimeOff = new TimeOffRequestDTO
            {
                Reason = "test",
                Type = RequestType.NonPaid,
                StartDate = DateTime.Now,
                EndDate = testDate
            };
            var result = await timeOffService.CreateTimeOffAsync(newTimeOff, TeamLeader.Id);
            Assert.True(result);

        }

        [Fact]
        public async Task GetAllTimeOffs_Creates_EmptyList()
        {
            var timeOffService = SetupMockedTimeOffService();
            var list = await timeOffService.GetAllAsync();

            Assert.Empty(list);
        }

        [Fact]
        public async Task GetAllTimeOffs_Creates_NonEmptyList()
        {
            var timeOffService = SetupMockedTimeOffService();
            var list = await timeOffService.GetAllAsync();
            list.Add(testTimeOff);

            Assert.NotEmpty(list);
        }

        [Fact]
        public async Task GetMyTimeOffs_Creates_EmptyList()
        {
            var timeOffService = SetupMockedTimeOffServiceForMyTimeOffsMethod();
            var list = await timeOffService.GetMyTimeOffs(defaultUser.Id);

            Assert.Empty(list);
        }

        [Fact]
        public async Task GetMyTimeOffs_Creates_NonEmptyList()
        {
            var timeOffService = SetupMockedTimeOffServiceForMyTimeOffsMethod();
            var list = await timeOffService.GetMyTimeOffs(defaultUser.Id);
            list.Add(testTimeOff);

            Assert.NotEmpty(list);
        }

        [Fact]
        public void GetTimeOff_IsSuccessful()
        {
            var timeOffService = SetupMockedTimeOffService();
            var newTimeOff = timeOffService.GetTimeOffAsync(testTimeOff.Id);

            Assert.NotNull(newTimeOff);
        }

        [Fact]
        public async Task GetTimeOff_IsNotSuccessful()
        {
            var timeOffService = SetupMockedTimeOffServiceForMyTimeOffsMethod();
            var newTimeOff = await timeOffService.GetTimeOffAsync(testTimeOff.Id);

            Assert.Null(newTimeOff);
        }

        [Fact]
        public async Task DeleteTimeOff_IsSuccessful()
        {


            var timeOffService = SetupMockedTimeOffService();
            var result = await timeOffService.DeleteTimeOffAsync(testTimeOff.Id);

            Assert.True(result);
        }

        [Fact]
        public async Task UpdateTimeOff_IsSuccessful()
        {
            EditTimeOffRequestDTO editTimeOff = new EditTimeOffRequestDTO
            {
                Reason = "test",
                StartDate = DateTime.Now,
                EndDate = testDate
            };

            var timeOffService = SetupMockedTimeOffService();
            var result = await timeOffService.EditTimeOffAsync(testTimeOff.Id, editTimeOff,defaultUser);

            Assert.True(result);
        }

        [Fact]
        public async Task SubmitFeedbackForTimeOffRequest_IsSuccessful_For_NonPaid()
        {
            var timeOffService = SetupMockedTimeOffService_For_NonPaid();
            var result = await timeOffService.SubmitFeedbackForTimeOffRequestAsync(TeamLeader, testTimeOffSubmitNonPaid.Id, (Status)3);

            Assert.True(result);
        }

        [Fact]
        public async Task SubmitFeedbackForTimeOffRequest_For_NonPaid_Rejected()
        {
            var timeOffService = SetupMockedTimeOffService_For_NonPaid();
            var result = await timeOffService.SubmitFeedbackForTimeOffRequestAsync(TeamLeader, testTimeOffSubmitNonPaid.Id, Status.Rejected);

            Assert.True(result);
        }

        [Fact]
        public async Task SubmitFeedbackForTimeOffRequest_IsSuccessful_For_NonPaid_WithReviewers()
        {
            var timeOffService = SetupMockedTimeOffService_For_Paid();
            testTimeOff.Reviewers.Add(defaultUser);

            var result = await timeOffService.SubmitFeedbackForTimeOffRequestAsync(TeamLeader, testTimeOffSubmitNonPaid.Id, Status.Awaiting);

            Assert.True(result);
        }

        [Fact]
        public async Task SubmitFeedbackForTimeOffRequest_IsSuccessful_For_Paid()
        {
            var timeOffService = SetupMockedTimeOffService_For_Paid();
            var result = await timeOffService.SubmitFeedbackForTimeOffRequestAsync(TeamLeader, testTimeOffSubmitPaid.Id, (Status)3);

            Assert.True(result);
        }

        [Fact]
        public async Task SubmitFeedbackForTimeOffRequest_IsSuccessful_For_Paid_Status_Rejected()
        {
            var timeOffService = SetupMockedTimeOffService_For_Paid();
            var result = await timeOffService.SubmitFeedbackForTimeOffRequestAsync(TeamLeader, testTimeOffSubmitPaid.Id, Status.Rejected);

            Assert.True(result);
        }

        [Fact]
        public async Task SubmitFeedbackForTimeOffRequest_IsSuccessful_For_Paid_WithReviewers()
        {
            var timeOffService = SetupMockedTimeOffService_For_Paid();
            testTimeOff.Reviewers.Add(defaultUser);

            var result = await timeOffService.SubmitFeedbackForTimeOffRequestAsync(TeamLeader, testTimeOffSubmitPaid.Id, Status.Awaiting);
            Assert.True(result);
        }

        [Fact]
        public async Task Create_TimeOff_SickLeave_Successfully_ReturnsTrue()
        {
            var timeOffService = SetupMockedTimeOffService();
            TimeOffRequestDTO newTimeOff = new TimeOffRequestDTO
            {
                Reason = "test",
                Type = RequestType.SickLeave,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(+4),
            };

            var result = await timeOffService.CreateTimeOffAsync(newTimeOff, TeamLeader.Id);
            Assert.True(result);
        }

        [Fact]
        public async Task Create_TimeOff_NonPaid_Successfully_ReturnsTrue()
        {
            var timeOffService = SetupMockedTimeOffService();
            TimeOffRequestDTO newTimeOff = new TimeOffRequestDTO
            {
                Reason = "test",
                Type = RequestType.NonPaid,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(+4),
            };

            var result = await timeOffService.CreateTimeOffAsync(newTimeOff, TeamLeader.Id);
            Assert.True(result);
        }

        [Fact]
        public async Task Create_TimeOff_Paid_Successfully_ReturnsTrue()
        {
            var timeOffService = SetupMockedTimeOffService();
            TimeOffRequestDTO newTimeOff = new TimeOffRequestDTO
            {
                Reason = "test",
                Type = RequestType.Paid,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(+4),
            };

            var result = await timeOffService.CreateTimeOffAsync(newTimeOff, defaultUser.Id);
            Assert.True(result);
        }

        [Fact]
        public void GetDaysTaken_Returns_Int()
        {
            var timeOffService = SetupMockedTimeOffService();

            var firstTimeOff = new TimeOff() 
            { 
                Status = Status.Approved,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(+4)
            };

            var secondTimeOff = new TimeOff()
            {
                Status = Status.Approved,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(+4)
            };

            int result = timeOffService.GetDaysTaken(new List<TimeOff>() { firstTimeOff, secondTimeOff });
            Assert.IsType<int>(result);
        }

        [Fact]
        public void GetHolidaysFromCurrentRequest_Returns_Int()
        {
            var timeOffService = SetupMockedTimeOffService();

            var firstTimeOff = new TimeOff()
            {
                Status = Status.Approved,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(+4)
            };

            int result = timeOffService.GetHolidaysFromCurrentRequest(firstTimeOff);
            Assert.IsType<int>(result);
        }

        [Fact]
        public void GetOffDays_Returns_OffDaysDTO()
        {
            var timeOffService = SetupMockedTimeOffService();

            var result = timeOffService.GetOffDays(defaultUser);

            Assert.IsType<OffDaysDTO>(result);
        }
    }
}
