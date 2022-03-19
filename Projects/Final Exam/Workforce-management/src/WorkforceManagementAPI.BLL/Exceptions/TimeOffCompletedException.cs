using System;

namespace WorkforceManagementAPI.BLL.Exceptions
{
    public class TimeOffCompletedException : Exception
    {
        public TimeOffCompletedException(string message) : base (message)
        {

        }
    }
}
