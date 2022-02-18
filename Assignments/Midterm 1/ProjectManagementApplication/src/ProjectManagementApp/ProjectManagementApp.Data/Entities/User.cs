using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.Data.Entities
{
    public class User : Entity
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(50)]
        public string Password { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(50)]
        public bool IsAdmin { get; set; }

        public virtual List<TeamUser> TeamUsers { get; set; }
        public virtual List<Project> Projects { get; set; }
        public virtual List<TaskProj> OwnedTasks { get; set; }
        public virtual List<TaskProj> AssignedTasks { get; set; }
        public virtual List<WorkLog> WorkLogs { get; set; }
    }
}
