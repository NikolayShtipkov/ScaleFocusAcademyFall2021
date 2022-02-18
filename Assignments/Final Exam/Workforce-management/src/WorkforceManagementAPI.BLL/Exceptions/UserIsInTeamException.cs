using System;

namespace WorkforceManagementAPI.BLL.Exceptions
{
    public class UserIsInTeamException : Exception
    {
        public UserIsInTeamException(string message) : base(message)
        {

        }
    }
}
