using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.Data.DTO.Requests
{
    public class WorkLogEditRequestDTO
    {
        [Required]
        public int TimeWorked { get; set; }
    }
}
