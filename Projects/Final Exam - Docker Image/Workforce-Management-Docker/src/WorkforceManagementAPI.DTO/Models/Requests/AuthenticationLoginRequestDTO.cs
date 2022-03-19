using System.ComponentModel.DataAnnotations;

namespace WorkforceManagementAPI.DTO.Models.Requests
{
    public class AuthenticationLoginRequestDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
