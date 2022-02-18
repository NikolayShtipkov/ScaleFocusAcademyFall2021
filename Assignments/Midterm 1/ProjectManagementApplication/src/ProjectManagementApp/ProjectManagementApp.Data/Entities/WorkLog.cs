using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.Data.Entities
{
    public class WorkLog : Entity
    {
        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int TimeWorked { get; set; }

        [ForeignKey("TaskId")]
        public virtual TaskProj Task { get; set; }
        public int TaskId { get; set; }
    }
}
