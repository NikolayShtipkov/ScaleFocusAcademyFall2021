using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DTO.Models.Requests;
using Xunit;

namespace WorkforceManagementAPI.Test
{
    public class UserServiceTest : ServicesTestBase
    {
        [Fact]
        public async Task Create_User_ReturnsTrue()
        {
            var userService = SetupMockedDefaultUserService();
            var userRequest = new CreateUserRequestDTO
            {
                Email = "test@abv.bg",
                FirstName = "test",
                LastName = "tester",
                Password = "123456789",
                RepeatPassword = "123456789"
            };

            var result = await userService.CreateUser(userRequest);

            Assert.True(result);
        }

        [Fact]
        public async Task Update_User_ReturnsTrue()
        {
            var userService = SetupMockedDefaultUserServiceWithDefaultUser();
            var EditUserRequest = new EditUserRequest
            {
                NewEmail = "test@abv.bg",
                NewFirstName = "test",
                NewLastName = "tester",
                NewPassword = "123456789",
                RepeatPassword = "123456789"
            };

            var result = await userService.UpdateUser(this.defaultUser.Id, EditUserRequest);

            Assert.True(result);
        }

        [Fact]
        public async Task Delete_User_ReturnsTrue()
        {
            var userService = SetupMockedDefaultUserServiceWithDefaultUser();

            var result = await userService.DeleteUser(this.defaultUser.Id);

            Assert.True(result);
        }

        [Fact]
        public async Task Set_UserAsAdmin_Success()
        {
            var userService = SetupMockedDefaultUserServiceWithDefaultUser();

            var result = await Record.ExceptionAsync(() => userService.SetAdministrator(this.defaultUser.Id));

            Assert.Null(result);
        }

        [Fact]
        public async Task Get_UserById_ReturnsUser()
        {
            var userService = SetupMockedDefaultUserServiceWithDefaultUser();

            var result = await userService.GetUserById(this.defaultUser.Id);

            Assert.Equal(typeof(User), result.GetType());
        }

        [Fact]
        public async Task GetAll_Users_ReturnsListOfUser()
        {
            var userService = SetupMockedDefaultUserServiceWithDefaultUsers();

            var result = await userService.GetAll();

            Assert.Equal(typeof(List<User>), result.GetType());
        }

        [Fact]
        public void GetCurrent_User_ReturnsNotNull()
        {
            var userService = SetupMockedDefaultUserServiceWithDefaultUser();
            var fackClaimPrinciple = new Mock<ClaimsPrincipal>();
            IEnumerable<Claim> claims = new List<Claim>() {
            new Claim(ClaimTypes.Name, "user@abv.bg")
            }.AsEnumerable();
            fackClaimPrinciple.Setup(e => e.Claims).Returns(claims);
            Thread.CurrentPrincipal = fackClaimPrinciple.Object;

            var result = userService.GetCurrentUser(fackClaimPrinciple.Object);

            Assert.NotNull(result);
        }


    }
}
