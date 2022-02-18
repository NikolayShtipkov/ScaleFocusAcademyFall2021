using Moq;
using System;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Exceptions;
using WorkforceManagementAPI.BLL.Services;
using Xunit;
using WorkforceManagementAPI.DAL.Entities.Enums;
using WorkforceManagementAPI.DAL.Contracts.IdentityContracts;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.BLL.Contracts;

namespace WorkforceManagementAPI.Test
{
    public class ValidationServiceTest : ServicesTestBase
    {
        [Fact]
        public void EnsureTeamExist_Must_Throw_Exception_When_Team_Null()
        {
            var validation = SetupMockedDefaultValidationService();

            Assert.Throws<EntityNotFoundException>(() => validation.EnsureTeamExist(null));
        }

        [Fact]
        public void EnsureTeamExist_Must_Throw_NoException_When_Team_Is_Valid()
        {
            var validation = SetupMockedDefaultValidationService();

            var ex = Record.Exception(() => validation.EnsureTeamExist(new Team()));

            Assert.Null(ex);
        }

        [Fact]
        public void EnsureUserExist_Must_Throw_Exception_When_User_Null()
        {
            var validation = SetupMockedDefaultValidationService();

            Assert.Throws<EntityNotFoundException>(() => validation.EnsureUserExist(null));
        }

        [Fact]
        public void EnsureUserExist_Must_Throw_NoException_When_User_IsValid()
        {
            var validation = SetupMockedDefaultValidationService();

            var ex = Record.Exception(() => validation.EnsureUserExist(new User()));

            Assert.Null(ex);
        }

        [Fact]
        public void EnsureTimeOffExist_Must_Throw_Exception_When_TimeOff_Null()
        {
            var validation = SetupMockedDefaultValidationService();

            Assert.Throws<EntityNotFoundException>(() => validation.EnsureTimeOffExist(null));
        }

        [Fact]
        public void EnsureTimeOffExist_Must_Throw_NoException_When_TimeOff_IsValid()
        {
            var validation = SetupMockedDefaultValidationService();

            var ex = Record.Exception(() => validation.EnsureTimeOffExist(new TimeOff()));

            Assert.Null(ex);
        }

        [Fact]
        public void EnsureLenghtIsValid_Must_Throw_Exception_When_Length_Invalid()
        {
            var validation = SetupMockedDefaultValidationService();

            Assert.Throws<InvalidLengthException>(() => validation.EnsureLenghtIsValid("1", 2, "1"));
        }

        [Fact]
        public void EnsureLenghtIsValid_Must_Throw_NoException_When_Length_IsValid()
        {
            var validation = SetupMockedDefaultValidationService();

            var ex = Record.Exception(() => validation.EnsureLenghtIsValid("testing", 5, "testing"));

            Assert.Null(ex);
        }

        [Fact]
        public void EnsureMailIsValid_Must_Throw_Exception_When_Mail_Invalid()
        {
            var validation = SetupMockedDefaultValidationService();

            Assert.Throws<InvalidEmailException>(() => validation.EnsureEmailIsValid("mail"));
        }

        [Fact]
        public void EnsureMailIsValid_Must_Throw_NoException_When_Mail_IsValid()
        {
            var validation = SetupMockedDefaultValidationService();

            var ex = Record.Exception(() => validation.EnsureEmailIsValid("test@abv.bg"));

            Assert.Null(ex);
        }

        [Fact]
        public async Task EnsureEmailIsUniqueAsync_Must_Throw_Exception_When_Mail_Invalid()
        {
            var validation = SetupMockedDefaultValidationService();

            await Assert.ThrowsAsync<EmailAlreadyInUseException>(() => validation.EnsureEmailIsUniqueAsync("mail"));
        }

        [Fact]
        public async Task EnsureEmailIsUniqueAsync_Must_Throw_NoException_When_Mail_IsValid()
        {
            var validation = SetupMockedDefaultValidationService();

            var ex = Record.ExceptionAsync(() => validation.EnsureEmailIsUniqueAsync("test@abv.bg")).AsyncState;

            Assert.Null(ex);
        }

        [Fact]
        public void CheckTeamName_Must_Throw_Exception_When_Team_ExistAsync()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            Assert.Throws<NameExistException>(() => validation.EnsureTeamNameIsUnique(regularTeam.Title));
        }

        [Fact]
        public void CheckTeamName_Must_Throw_NoException_When_Team_ExistAsync()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();
            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            var ex = Record.Exception(() => validation.EnsureTeamNameIsUnique("test1"));

            Assert.Null(ex);
        }

        [Fact]
        public void EnsureInputFitsBoundaries_Must_Throw_Exception_When_Input_Invalid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            Assert.Throws<InputOutOfBoundsException>(() => validation.EnsureInputFitsBoundaries(10, 0, 5));
        }

        [Fact]
        public void EnsureInputFitsBoundaries_Must_Throw_NoException_When_Input_IsValid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            var ex = Record.Exception(() => validation.EnsureInputFitsBoundaries(4, 0, 5));

            Assert.Null(ex);
        }

        [Fact]
        public void EnsureInputFitsBoundaries_Must_Throw_Exception_When_Date_Invalid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            Assert.Throws<InputOutOfBoundsException>(() => validation.EnsureInputFitsBoundaries(new DateTime(2022, 1, 14), new DateTime(2022, 1, 15), new DateTime(2023, 1, 1)));
        }

        [Fact]
        public void EnsureInputFitsBoundaries_Must_Throw_NoException_When_Date_IsValid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            var ex = Record.Exception(() => validation.EnsureInputFitsBoundaries(new DateTime(2022, 1, 20), new DateTime(2022, 1, 15), new DateTime(2022, 1, 25)));

            Assert.Null(ex);
        }

        [Fact]
        public void ValidateDateRange_Must_Throw_Exception_When_Date_Invalid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            Assert.Throws<InputOutOfBoundsException>(() => validation.EnsureDateRangeIsValid(new DateTime(2022, 1, 18), new DateTime(2022, 1, 15)));
        }

        [Fact]
        public void ValidateDateRange_Must_Throw_NoException_When_Date_IsValid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            var ex = Record.Exception(() => validation.EnsureDateRangeIsValid(new DateTime(2022, 3, 30), new DateTime(2022, 3, 31)));

            Assert.Null(ex);
        }

        [Fact]
        public void ValidateDateRange_Must_Throw_Exception_When_Date_IsInvalid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new Mock<IValidationService>();
            validation.Setup(m => m.EnsureDateRangeIsValid(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Throws<ArgumentException>();
            Assert.Throws<ArgumentException>(() => validation.Object.EnsureDateRangeIsValid(new DateTime(2022, 1, 18), new DateTime(2022, 1, 15)));
        }

        [Fact]
        public void CheckAccessToTeam_Must_Throw_Exception_When_User_Invalid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            Assert.Throws<UnauthorizedUserException>(() => validation.EnsureUserHasAccessToTeam(regularTeam, defaultUser));
        }

        [Fact]
        public void CheckAccessToTeam_Must_Throw_NoException_When_User_IsValid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            var ex = Record.Exception(() => validation.EnsureUserHasAccessToTeam(regularTeam, TeamLeader));

            Assert.Null(ex);
        }

        [Fact]
        public void EnsureUnassignUserHasAccessToTeam_Must_Throw_Exception_When_User_Invalid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            Assert.Throws<UnauthorizedUserException>(() => validation.EnsureUnassignUserHasAccessToTeam(regularTeam, defaultUser));
        }

        [Fact]
        public void EnsureUnassignUserHasAccessToTeam_Must_Throw_NoException_When_User_IsValid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            var ex = Record.Exception(() => validation.EnsureUnassignUserHasAccessToTeam(regularTeam, TeamLeader));

            Assert.Null(ex);
        }

        [Fact]
        public void CheckTeamLeader_Must_Throw_Exception_When_User_Invalid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            Assert.Throws<UserAlreadyTeamLeaderException>(() => validation.EnsureUserIsNotAlreadyATeamLeader(regularTeam, TeamLeader));
        }

        [Fact]
        public void CheckTeamLeader_Must_Throw_No_Exception_When_User_IsNot_TeamLeader()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            var ex = Record.Exception(() => validation.EnsureUserIsNotAlreadyATeamLeader(regularTeam, defaultUser));
            Assert.Null(ex);
        }

        [Fact]
        public void CheckTeamLeader_Must_Throw_NoException_When_User_IsValid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);
            regularTeam.Users.Add(defaultUser);

            var ex = Record.Exception(() => validation.EnsureUserHasAccessToTeam(regularTeam, defaultUser));

            Assert.Null(ex);
        }

        [Fact]
        public void CanAddToTeam_Must_Throw_Exception_When_User_Invalid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            Assert.Throws<UserAlreadyInTeamException>(() => validation.EnsureUserIsNotAlreadyPartOfTheTeam(regularTeam, TeamLeader));
        }

        [Fact]
        public void CanAddToTeam_Must_Throw_NoException_When_User_IsValid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            var ex = Record.Exception(() => validation.EnsureUserHasAccessToTeam(regularTeam, TeamLeader));

            Assert.Null(ex);
        }

        [Fact]
        public void EnsureNoReviewersLeft_Must_Throw_Exception_When_TimeOff_Invalid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            Assert.Throws<RequestAlreadyCompletedException>(() => validation.EnsureNoReviewersLeft(noReviewersTimeOff));
        }

        [Fact]
        public void EnsureNoReviewersLeft_Must_Throw_NoException_When_TimeOff_IsValid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);
            testTimeOff.Reviewers.Add(defaultUser);
            var ex = Record.Exception(() => validation.EnsureNoReviewersLeft(testTimeOff));

            Assert.Null(ex);
        }

        [Fact]
        public void EnsureUserIsReviewer_Must_Throw_Exception_When_User_Invalid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            Assert.Throws<UnauthorizedUserException>(() => validation.EnsureUserIsReviewer(testTimeOff, defaultUser));
        }

        [Fact]
        public void EnsureUserIsReviewer_Must_Throw_NoException_When_User_IsValid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);
            testTimeOff.Reviewers.Add(defaultUser);
            var ex = Record.Exception(() => validation.EnsureUserIsReviewer(testTimeOff, defaultUser));

            Assert.Null(ex);
        }

        [Fact]
        public void EnsureResponseIsValid_Must_Throw_Exception_When_Input_Invalid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            Assert.Throws<InputOutOfBoundsException>(() => validation.EnsureResponseIsValid(Status.Awaiting));
        }

        [Fact]
        public void EnsureResponseIsValid_Must_Throw_NoException_When_Input_IsValid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            var ex = Record.Exception(() => validation.EnsureResponseIsValid(Status.Approved));

            Assert.Null(ex);
        }

        [Fact]
        public void EnsureResponseIsValid_Must_Throw_Exception_When_Input_Invalid_Second_Case()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            Assert.Throws<InputOutOfBoundsException>(() => validation.EnsureResponseIsValid(Status.Created));
        }

        [Fact]
        public void EnsureUserIsAdminAsync_Must_Throw_Exception_When_Input_Invalid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            Assert.ThrowsAsync<UserAlreadyAnAdminException>(() => validation.EnsureUserIsAdminAsync(defaultUser));
        }

        [Fact]
        public void EnsureUserIsAdminAsync_Must_Throw_NoException_When_Input_IsValid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);
            mockedManager.Object.AddUserToRoleAsync(defaultUser, "Adming");
            var ex = Record.ExceptionAsync(() => validation.EnsureUserIsAdminAsync(defaultUser)).AsyncState;

            Assert.Null(ex);
        }

        [Fact]
        public void EnsureUserIsAdminAsync_Must_Throw_NoException_When_Role_IsCorrect()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);
            mockedManager.Object.AddUserToRoleAsync(defaultUser, "Admin");
            var ex = Record.ExceptionAsync(() => validation.EnsureUserIsAdminAsync(defaultUser)).AsyncState;

            Assert.Null(ex);
        }

        [Fact]
        public void EnsureUpdateEmailIsUniqueAsync_Must_Throw_Exception_When_Input_Invalid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            Assert.ThrowsAsync<EmailAlreadyInUseException>(() => validation.EnsureUpdateEmailIsUniqueAsync("mail", defaultUser));
        }

        [Fact]
        public void EnsureUpdateEmailIsUniqueAsync_Must_Throw_NoException_When_Input_IsValid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            var ex = Record.ExceptionAsync(() => validation.EnsureUpdateEmailIsUniqueAsync("testing@abv.bg", defaultUser)).AsyncState;

            Assert.Null(ex);
        }

        [Fact]
        public void EnsureUpdateEmailIsUniqueAsync_Must_Throw_NoException_When_Input_IsOldEmail()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            var ex = Record.ExceptionAsync(() => validation.EnsureUpdateEmailIsUniqueAsync("test@abv.bg", defaultUser)).AsyncState;

            Assert.Null(ex);
        }

        [Fact]
        public void EnsureUserIsNotInTeam_Must_Throw_Exception_When_Input_Invalid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);
            defaultUser.Teams.Add(new Team());

            Assert.Throws<UserIsInTeamException>(() => validation.EnsureUserIsNotInTeam(defaultUser));
        }

        [Fact]
        public void EnsureUserIsNotInTeam_Must_Throw_NoException_When_Input_IsValid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            var ex = Record.Exception(() => validation.EnsureUserIsNotInTeam(defaultUser));

            Assert.Null(ex);
        }

        [Fact]
        public void EnsureTeamNameIsUniqueWhenEdit_Must_Throw_Exception_When_Input_Invalid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            Assert.Throws<NameExistException>(() => validation.EnsureTeamNameIsUniqueWhenEdit(regularTeam.Title, "newTitle"));
        }

        [Fact]
        public void EnsureTeamNameIsUniqueWhenEdit_Must_Throw_NoException_When_Input_IsValid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);
            var team = new Team {
                Title = "new" };

            var ex = Record.Exception(() => validation.EnsureTeamNameIsUniqueWhenEdit(team.Title, "new"));

            Assert.Null(ex);
        }
        public void EnsureResponseIsValid_Must_Throw_Exception_When_Input_Invalid_SecondCase()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            Assert.Throws<InputOutOfBoundsException>(() => validation.EnsureResponseIsValid(Status.Rejected));
        }

        [Fact]
        public void EnsureMailIsValid_Must_Throw_Exception_When_Mail_Invalid_SecondCase()
        {
            var validation = SetupMockedDefaultValidationService();

            Assert.Throws<InvalidEmailException>(() => validation.EnsureEmailIsValid("mail@@.abv"));
        }

        [Fact]
        public void EnsureInputFitsBoundaries_Must_Throw_Exception_When_Input_Invalid_SecondCase()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            Assert.Throws<InputOutOfBoundsException>(() => validation.EnsureInputFitsBoundaries(1, 2, 5));
        }

        [Fact]
        public void EnsureInputFitsBoundaries_Must_Throw_Exception_When_Date_Invalid_SecondCase()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            Assert.Throws<InputOutOfBoundsException>(() => validation.EnsureInputFitsBoundaries(new DateTime(2022, 1, 14), new DateTime(2022, 1, 10), new DateTime(2022, 1, 13)));
        }

        [Fact]
        public void EnsureUserHasEnoughDays_Throws_Exception()
        {
            var validation = SetupMockedDefaultValidationService();

            Assert.Throws<NotEnoughDaysForTimeOffException>(() => validation.EnsureUserHasEnoughDays(19, 3));
        }

        [Fact]
        public void EnsureUserHasEnoughDays_Throws_NoException_WhenValid()
        {
            var validation = SetupMockedDefaultValidationService();

            var ex = Record.Exception(() => validation.EnsureUserHasEnoughDays(10, 5));

            Assert.Null(ex);
        }

        [Fact]
        public void EnsureUserHasEnoughDays_Throws_No_Exception()
        {
            var validation = SetupMockedDefaultValidationService();

            var exception = Record.Exception(() => validation.EnsureUserHasEnoughDays(3, 3));

            Assert.Null(exception);
        }

        [Fact]
        public void EnsureTodayIsWorkingDay_Throws_Exception()
        {
            var validation = SetupMockedDefaultValidationService();

            Assert.Throws<NotAWorkingDayException>(() => validation.EnsureTodayIsWorkingDay(new DateTime(2022, 03, 03)));
        }

        [Fact]
        public void EnsureTodayIsWorkingDay_Throws_NoException_WhenValid()
        {
            var validation = SetupMockedDefaultValidationService();

            var ex = Record.Exception(() => validation.EnsureTodayIsWorkingDay(new DateTime(2022, 03, 04)));

            Assert.Null(ex);
        }

        [Fact]
        public void EnsureTimeOffRequestsDoNotOverlap_Throws_Exception()
        {
            var validation = SetupMockedDefaultValidationService();

            defaultUser.Requests.Add(new TimeOff
            {
                Status = Status.Awaiting,
                StartDate = new DateTime(2022, 01, 01),
                EndDate = new DateTime(2022, 01, 10)
            });

            testTimeOff.Id = Guid.NewGuid();
            testTimeOff.Status = Status.Created;
            testTimeOff.StartDate = new DateTime(2022, 01, 02);
            testTimeOff.EndDate = new DateTime(2022, 01, 11);

            Assert.Throws<TimeOffOverlapException>(() => validation.EnsureTimeOffRequestsDoNotOverlap(defaultUser, testTimeOff));
        }

        [Fact]
        public void EnsureTimeOffRequestsDoNotOverlap_Throws_NoException_WhenValid()
        {
            var validation = SetupMockedDefaultValidationService();

            defaultUser.Requests.Add(new TimeOff
            {
                Status = Status.Awaiting,
                StartDate = new DateTime(2022, 01, 01),
                EndDate = new DateTime(2022, 01, 10)
            });

            testTimeOff.Status = Status.Created;
            testTimeOff.StartDate = new DateTime(2022, 10, 02);
            testTimeOff.EndDate = new DateTime(2022, 10, 11);

            var ex = Record.Exception(() => validation.EnsureTimeOffRequestsDoNotOverlap(defaultUser, testTimeOff));

            Assert.Null(ex);
        }

        [Fact]
        public void EnsureTimeOffRequestIsNotCompleted_Must_Throw_Exception_When_Input_Invalid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);
            testTimeOff.Status = Status.Approved;

            Assert.Throws<TimeOffCompletedException>(() => validation.EnsureTimeOffRequestIsNotCompleted(testTimeOff));
        }

        [Fact]
        public void EnsureTimeOffRequestIsNotCompleted_Must_Throw_NoException_When_Input_IsValid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);
            testTimeOff.Status = Status.Created;

            var ex = Record.Exception(() => validation.EnsureTimeOffRequestIsNotCompleted(testTimeOff));

            Assert.Null(ex);
        }

        [Fact]
        public void CheckTimeOffStatus_Throws_Exception()
        {
            var validation = SetupMockedDefaultValidationService();

            testTimeOff.Status = Status.Awaiting;

            Assert.Throws<RequestAlreadyCompletedException>(() => validation.CheckTimeOffStatus(testTimeOff));
        }

        [Fact]
        public void CheckTimeOffStatus_Is_Valid()
        {
            var validation = SetupMockedDefaultValidationService();

            testTimeOff.Status = Status.Approved;

            var ex = Record.Exception(() => validation.CheckTimeOffStatus(testTimeOff));

            Assert.Null(ex);
        }

        [Fact]
        public void EnsureUserDoesntHaveRequests_Throws_Exception()
        {
            var validation = SetupMockedDefaultValidationService();
            defaultUser.Requests.Add(new TimeOff());
            Assert.Throws<UserHasExistingRequestsException>(() => validation.EnsureUserDoesntHaveRequests(defaultUser));
        }

        [Fact]
        public void EnsureUserDoesntHaveRequests_Is_Valid()
        {
            var validation = SetupMockedDefaultValidationService();

            var ex = Record.Exception(() => validation.EnsureUserDoesntHaveRequests(defaultUser));

            Assert.Null(ex);
        }

    }
}
