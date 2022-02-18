using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.Data.Entities
{
    public class Project : Entity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [ForeignKey("OwnerId")]
        public virtual User Owner { get; set; }
        public int OwnerId { get; set; }

        public virtual List<TaskProj> Tasks { get; set; }
        public virtual List<ProjectTeam> ProjectTeams { get; set; }
    }
}
