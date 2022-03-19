using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListApp.Entities
{
    public abstract class Entity
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

        public int CreatorId { get; set; }

        public int ModifierId { get; set; }

        public virtual User Creator { get; set; }

        public virtual User Modifier { get; set; }
    }
}
