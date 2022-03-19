using System;
using System.Collections.Generic;
using WorkforceManagementAPI.DAL.Entities.Enums;

namespace WorkforceManagementAPI.DAL.Entities
{
    public class TimeOff
    {
        public Guid Id { get; set; }

        public string Reason { get; set; }

        public RequestType Type { get; set; }

        public Status Status { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

        public string CreatorId { get; set; }

        public virtual User Creator { get; set; }

        public string ModifierId { get; set; }

        public virtual User Modifier { get; set; }

        public virtual List<User> Reviewers { get; set; }

        public TimeOff()
        {
            Reviewers = new List<User>();
        }
    }
}
