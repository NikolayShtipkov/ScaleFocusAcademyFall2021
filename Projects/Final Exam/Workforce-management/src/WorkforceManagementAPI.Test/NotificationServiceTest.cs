using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Services;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DTO.Models;
using Xunit;

namespace WorkforceManagementAPI.Test
{
    public class NotificationServiceTest : ServicesTestBase
    {
        [Fact]
        public void Send_Should_Not_Throw_Exception_When_Users_Valid()
        {
            var mockOptions = new Mock<IOptions<MailSettings>>();
            var service = new NotificationService(mockOptions.Object);

            var ex = Record.ExceptionAsync(() => service.Send(new List<User>() { TeamLeader }, "text", "text"));
            Assert.Equal(ex.Status.ToString(), TaskStatus.RanToCompletion.ToString());
        }

        [Fact]
        public async Task Send_Should_Throw_Exception_When_Users_Invalid()
        {
            var mockOptions = new Mock<IOptions<MailSettings>>();
            var service = new NotificationService(mockOptions.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.Send(null, "text", "text"));
        }

        [Fact]
        public async Task Send_Should_Throw_Exception_When_Users_Empty()
        {
            var mockOptions = new Mock<IOptions<MailSettings>>();
            var service = new NotificationService(mockOptions.Object);

            await Assert.ThrowsAsync<NullReferenceException>(() => service.Send(new List<User>(), "text", "text"));
        }
    }
}
