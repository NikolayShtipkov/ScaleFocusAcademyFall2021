namespace WorkforceManagementAPI.Common
{
    public static class Constants
    {
        public const string NotFound = "{0} not found";

        public const string NameAlreadyInUse = "{0} is already in use.";

        public const string InvalidLength = "The field {0} must have a minimum length of {1}";

        public const string InvalidEmail = "The email must be valid";

        public const string EmailAreadyInUse = "The email already exists";

        public const string Created = "{0} was created";

        public const string Deleted = "{0} was deleted";

        public const string OperationFailed = "Error!! Operation Failed";

        public const string InvalidInput = "Invalid input";

        public const string RequestMessage = "Dear Team Lead\n I ({0} {1}) want to leave from {2} to {3} for a {4} vacation because of {5}";

        public const string SickMessage = "Dear Team Lead\n I ({0} {1}) am writing this letter to inform you that I need to take sick leave from work. I will need to remain off work from {2} to {3} because of {4}";

        public const string InputOutOfBounds = "{0} input is exceeds predifined boundaries";

        public const string TeamAccess = "Can't assign user as a leader in a team where they are not a member of.";

        public const string InvalidTeamLeader = "User is already the assigned team leader.";

        public const string UserAlreadyMember = "User is already a member.";

        public const string AnswerToRequest = "Answer to request has been submited.";

        public const string CompletedRequest = "Time off request is already completed.";

        public const string NotReviewer = "User is not a reviewer.";

        public const string InvalidStatus = "Invalid status.";

        public const string UserIsAdmin = "User is already an admin";
    }
}
