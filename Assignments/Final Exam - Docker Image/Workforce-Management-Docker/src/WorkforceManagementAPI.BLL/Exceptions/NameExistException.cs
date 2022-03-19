using System;

namespace WorkforceManagementAPI.BLL.Exceptions
{
    public class NameExistException : Exception
    {
        public NameExistException(string message) : base(message)
        {

        }
    }
}
