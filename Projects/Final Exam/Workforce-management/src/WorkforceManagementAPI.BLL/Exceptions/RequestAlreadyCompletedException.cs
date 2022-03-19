using System;

namespace WorkforceManagementAPI.BLL.Exceptions
{
    public class RequestAlreadyCompletedException : Exception
    {
        public RequestAlreadyCompletedException(string message) : base(message)
        {

        }
    }
}
