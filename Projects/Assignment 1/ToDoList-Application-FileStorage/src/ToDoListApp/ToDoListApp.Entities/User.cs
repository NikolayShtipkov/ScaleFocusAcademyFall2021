using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListApp.Entities
{
    public class User : Person
    {  
        public User()
        {
            ToDoListIds = new List<int>();
            SharedToDoListIds = new List<int>();
        }

        public List<int> ToDoListIds { get; set; }

        public List<int> SharedToDoListIds { get; set; }

        public bool IsAdmin { get; set; }

    }
}
