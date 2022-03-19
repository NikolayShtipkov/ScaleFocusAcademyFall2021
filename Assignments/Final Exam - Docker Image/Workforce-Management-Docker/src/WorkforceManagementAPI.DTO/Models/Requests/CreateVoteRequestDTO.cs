using System.ComponentModel.DataAnnotations;
using WorkforceManagementAPI.DAL.Entities.Enums;

namespace WorkforceManagementAPI.DTO.Models.Requests
{
    public class CreateVoteRequestDTO
    {
        [Required]
        public Status Status { get; set; }
    }
}
