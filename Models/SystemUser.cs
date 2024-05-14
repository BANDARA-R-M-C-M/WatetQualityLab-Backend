using Microsoft.AspNetCore.Identity;

namespace Project_v1.Models
{
    public class SystemUser : IdentityUser
    {
        public string? LabID { get; set; }
        public virtual Lab? Lab { get; set; }
        public string? MOHAreaId { get; set; }
        public virtual MOHArea? MOHArea { get; set; }
        public string? PHIAreaId { get; set; }
        public virtual PHIArea? PHIArea { get; set; }
    }
}