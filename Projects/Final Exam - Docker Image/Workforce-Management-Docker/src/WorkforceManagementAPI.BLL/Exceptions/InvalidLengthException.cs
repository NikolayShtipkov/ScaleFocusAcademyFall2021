using System;

namespace WorkforceManagementAPI.BLL.Exceptions
{
    public class InvalidLengthException : Exception
    {
        public InvalidLengthException(string message) : base(message)
        {

        }
    }
}
