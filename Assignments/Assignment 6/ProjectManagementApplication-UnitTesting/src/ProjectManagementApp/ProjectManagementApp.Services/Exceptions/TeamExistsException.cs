using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.Service.Exceptions
{
    public class TeamExistsException : Exception
    {
        public TeamExistsException(string message) : base(message)
        {
        }
    }
}
