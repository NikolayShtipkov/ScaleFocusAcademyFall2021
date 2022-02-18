using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.DTO.Requests
{
    public class TaskProjCreateRequestDTO
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public int AssigneeId { get; set; }

        [Required]
        public int ProjectId { get; set; }

        public bool IsCompleted { get; set; }
    }
}
