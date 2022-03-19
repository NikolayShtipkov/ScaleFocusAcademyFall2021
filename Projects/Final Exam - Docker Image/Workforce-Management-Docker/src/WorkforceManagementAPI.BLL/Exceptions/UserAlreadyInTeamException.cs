using System;

namespace WorkforceManagementAPI.BLL.Exceptions
{
    public class UserAlreadyInTeamException : Exception
    {
        public UserAlreadyInTeamException(string message) : base(message)
        {

        }
    }
}
