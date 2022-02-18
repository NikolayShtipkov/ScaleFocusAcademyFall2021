using System;

namespace WorkforceManagementAPI.BLL.Exceptions
{
    public class UserHasExistingRequestsException : Exception
    {
        public UserHasExistingRequestsException(string message) : base (message)
        {

        }
    }
}
