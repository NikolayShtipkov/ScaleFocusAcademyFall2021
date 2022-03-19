using System;

namespace WorkforceManagementAPI.BLL.Exceptions
{
    public class UnauthorizedUserException : Exception
    {
        public UnauthorizedUserException(string message) : base(message)
        {

        }
    }
}
