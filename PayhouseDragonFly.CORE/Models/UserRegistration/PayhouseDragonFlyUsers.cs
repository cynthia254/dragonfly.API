using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayhouseDragonFly.CORE.Models.UserRegistration
{
    public  class PayhouseDragonFlyUsers:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PartnerName { get; set; }
        public bool EmailConfirmed { get; set; }
        public DateTime VerificationTime { get; set; }
        public string VerificationToken { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public int RoleId { get; set; }
    }
}
