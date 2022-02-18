﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.Data.DTO.Requests
{
    public class TaskProjEditRequestDTO
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int AssigneeId { get; set; }
    }
}
