using System;
using AutoMapper;
using Moq;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL.Entities.Enums;
using WorkforceManagementAPI.BLL.Contracts;
using WorkforceManagementAPI.BLL.Services;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DTO.Models.Requests;
using Xunit;

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
            TimeOffRequestDTO editTimeOff = new TimeOffRequestDTO
            {
                Reason = "test",
                Type = RequestType.NonPaid,
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
        public async Task SubmitFeedbackForTimeOffRequest_IsSuccessful_For_Paid()
        {
            var timeOffService = SetupMockedTimeOffService_For_Paid();
            var result = await timeOffService.SubmitFeedbackForTimeOffRequestAsync(TeamLeader, testTimeOffSubmitNonPaid.Id, (Status)3);

            Assert.True(result);
        }

        [Fact]
        public async Task SubmitFeedbackForTimeOffRequest_IsSuccessful_For_Paid_Status_Rejected()
        {
            var timeOffService = SetupMockedTimeOffService_For_Paid();
            var result = await timeOffService.SubmitFeedbackForTimeOffRequestAsync(TeamLeader, testTimeOffSubmitNonPaid.Id, Status.Rejected);

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


    }
}
