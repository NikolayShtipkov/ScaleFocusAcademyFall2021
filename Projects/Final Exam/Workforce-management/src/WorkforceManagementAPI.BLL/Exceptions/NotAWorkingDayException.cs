using System;

namespace WorkforceManagementAPI.BLL.Exceptions
{
    public class NotAWorkingDayException : Exception
    {
        public NotAWorkingDayException(string message) : base(message)
        {

        }
    }
}
