using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.Service.Exceptions
{
    public class TeamNotFoundException : Exception
    {
        public TeamNotFoundException(string message) : base(message)
        {
        }
    }
}
