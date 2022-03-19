using System;

namespace WorkforceManagementAPI.BLL.Exceptions
{
    public class InputOutOfBoundsException : Exception
    {
        public InputOutOfBoundsException(string message) : base(message)
        {

        }
    }
}
