﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.Service.Exceptions
{
    public class WorklogNotFoundException : Exception
    {
        public WorklogNotFoundException(string message) : base(message)
        {
        }
    }
}
