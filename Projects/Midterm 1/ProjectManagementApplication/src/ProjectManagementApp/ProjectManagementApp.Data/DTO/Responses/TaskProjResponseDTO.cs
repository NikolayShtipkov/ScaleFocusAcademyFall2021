using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.Data.DTO.Responses
{
    public class TaskProjResponseDTO : BaseResponseDTO
    {
        public string Name { get; set; }

        public int OwnerId { get; set; }

        public int AssigneeId { get; set; }

        public int ProjectId { get; set; }

        public string Status { get; set; }
    }
}
