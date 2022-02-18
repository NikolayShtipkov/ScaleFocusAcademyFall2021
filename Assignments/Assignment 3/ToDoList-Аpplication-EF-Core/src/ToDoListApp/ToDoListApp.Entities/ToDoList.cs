using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListApp.Entities
{
    public class ToDoList : Entity 
    {
        public string Title { get; set; }

        public virtual List<Task> Tasks { get; set; }

        public ToDoList()
        {
            Tasks = new List<Task>();
        }
    }
}
