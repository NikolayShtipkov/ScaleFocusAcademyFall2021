using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkforceManagementAPI.DTO.Models.Responses
{
    public class OffDaysDTO
    {
        public int AllDays { get; set; }

        public int Remaining { get; set; }

        public int UsedDays { get; set; }
    }
}
