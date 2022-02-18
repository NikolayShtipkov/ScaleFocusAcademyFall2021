using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.DTO.Requests
{
    public class TaskProjStatusRequestDTO
    {
        [Required]
        public bool IsCompleted { get; set; }
    }
}
