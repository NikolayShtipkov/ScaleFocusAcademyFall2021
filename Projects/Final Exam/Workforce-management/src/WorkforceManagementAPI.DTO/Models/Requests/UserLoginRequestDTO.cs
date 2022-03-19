using System.ComponentModel.DataAnnotations;

namespace WorkforceManagementAPI.DTO.Models.Requests
{
    public class UserLoginRequestDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
