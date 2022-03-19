using System;

namespace WorkforceManagementAPI.BLL.Exceptions
{
    public class UserAlreadyTeamLeaderException : Exception
    {
        public UserAlreadyTeamLeaderException(string message) : base(message)
        {

        }
    }
}
