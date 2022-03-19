using System;

namespace WorkforceManagementAPI.BLL.Exceptions
{
    public class TimeOffOverlapException : Exception
    {
        public TimeOffOverlapException(string message) : base(message)
        {

        }
    }
}
