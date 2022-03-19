using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.Data.Entities
{
    public class TaskProj : Entity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public bool IsCompleted { get; set; }

        [ForeignKey("OwnerId")]
        public virtual User Owner { get; set; }
        public int OwnerId { get; set; }

        [ForeignKey("AssigneeId")]
        public virtual User Assignee { get; set; }
        public int AssigneeId { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }
        public int ProjectId { get; set; }

        public virtual List<WorkLog> WorkLogs { get; set; }
    }
}
