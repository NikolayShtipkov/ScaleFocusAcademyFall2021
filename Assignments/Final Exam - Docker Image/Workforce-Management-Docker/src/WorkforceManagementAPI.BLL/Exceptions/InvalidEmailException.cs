using System;

namespace WorkforceManagementAPI.BLL.Exceptions
{
    public class InvalidEmailException : Exception
    {
        public InvalidEmailException(string message) : base(message)
        {

        }
    }
}
