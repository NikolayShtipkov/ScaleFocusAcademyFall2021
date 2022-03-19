using System;

namespace WorkforceManagementAPI.BLL.Exceptions
{
    public class NotEnoughDaysForTimeOffException : Exception
    {
        public NotEnoughDaysForTimeOffException(string message) : base(message)
        {

        }
    }
}
