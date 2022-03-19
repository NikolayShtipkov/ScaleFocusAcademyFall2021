using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.Data.Entities
{
    public class Team : Entity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual List<TeamUser> TeamUsers { get; set; }
        public virtual List<ProjectTeam> ProjectTeams { get; set; }
    }
}
