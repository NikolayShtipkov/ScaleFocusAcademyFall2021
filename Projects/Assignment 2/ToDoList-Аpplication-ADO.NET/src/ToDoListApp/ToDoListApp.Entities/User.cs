using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListApp.Entities
{
    public class User : Person
    { 
        public bool IsAdmin { get; set; }

    }
}
