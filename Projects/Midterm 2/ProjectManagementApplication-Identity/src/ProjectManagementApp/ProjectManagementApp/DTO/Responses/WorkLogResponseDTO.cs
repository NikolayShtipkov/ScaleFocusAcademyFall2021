using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.DTO.Responses
{
    public class WorkLogResponseDTO : BaseResponseDTO
    {
        public string CreateAt { get; set; }

        public int TimeWorked { get; set; }

        public int AssigneeId { get; set; }
    }
}
