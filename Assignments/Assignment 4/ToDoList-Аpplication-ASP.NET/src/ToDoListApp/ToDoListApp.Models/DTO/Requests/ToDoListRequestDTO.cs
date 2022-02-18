using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListApp.Models.DTO.Requests
{
    public class ToDoListRequestDTO
    {
        [Required]
        [MaxLength(20)]
        public string Title { get; set; }
    }
}
