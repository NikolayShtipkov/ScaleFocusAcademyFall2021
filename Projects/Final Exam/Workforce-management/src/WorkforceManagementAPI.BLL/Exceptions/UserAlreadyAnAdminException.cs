using System;

namespace WorkforceManagementAPI.BLL.Exceptions
{
    public class UserAlreadyAnAdminException : Exception
    {
        public UserAlreadyAnAdminException(string messege) : base(messege)
        {

        }


    }
}
