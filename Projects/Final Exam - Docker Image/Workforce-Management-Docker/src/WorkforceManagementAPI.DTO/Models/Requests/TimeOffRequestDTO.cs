using System;
using System.ComponentModel.DataAnnotations;
using WorkforceManagementAPI.DAL.Entities.Enums;

namespace WorkforceManagementAPI.DTO.Models.Requests
{
    public class TimeOffRequestDTO
    {   
        [Required]
        public string Reason { get; set; }

        [Required]
        public RequestType Type { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
