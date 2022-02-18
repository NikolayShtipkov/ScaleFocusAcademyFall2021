using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace WorkforceManagementAPI.DAL.Entities
{
    public class User : IdentityUser
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual List<Team> Teams { get; set; }

        public virtual List<TimeOff> Requests  { get; set; }

        public virtual List<TimeOff> UnderReviewRequests { get; set; }

        public User()
        {
            this.Teams = new List<Team>();
            this.Requests  = new List<TimeOff>();
            this.UnderReviewRequests = new List<TimeOff>();
        }
    }
}
