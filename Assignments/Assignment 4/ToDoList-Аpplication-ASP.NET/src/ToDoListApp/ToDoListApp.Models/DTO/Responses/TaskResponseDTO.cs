using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListApp.Models.DTO.Responses
{
    public class TaskResponseDTO : BaseResponseDTO
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsComplete { get; set; }

        public int ToDoListId { get; set; }
    }
}
