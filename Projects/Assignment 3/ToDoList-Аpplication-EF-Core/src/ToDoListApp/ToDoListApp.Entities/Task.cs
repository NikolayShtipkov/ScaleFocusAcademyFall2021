using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListApp.Entities
{
    public class Task : Entity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsComplete { get; set; }

        public virtual int ToDoListId { get; set; }

        public virtual ToDoList ToDoList { get; set; }
    }
}
