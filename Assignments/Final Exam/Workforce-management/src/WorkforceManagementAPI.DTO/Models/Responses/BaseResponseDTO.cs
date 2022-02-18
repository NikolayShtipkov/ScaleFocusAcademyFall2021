using System;

namespace WorkforceManagementAPI.DTO.Models.Responses
{
    public class BaseResponseDTO
    {
        public Guid Id { get; set; }

        public string CreatorId { get; set; }

        public string ModifierId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }
    }
}
