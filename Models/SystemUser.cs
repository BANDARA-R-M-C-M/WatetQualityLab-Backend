using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_v1.Models {
    public class SystemUser : IdentityUser {
        [Column(TypeName = "varchar(250)")]
        public string? ImageUrl { get; set; }

        [ForeignKey("Lab")]
        [Column(TypeName = "varchar(40)")]
        public string? LabID { get; set; }
        public virtual Lab? Lab { get; set; }

        [ForeignKey("MOHArea")]
        [Column(TypeName = "varchar(40)")]
        public string? MOHAreaId { get; set; }
        public virtual MOHArea? MOHArea { get; set; }

        [ForeignKey("PHIArea")]
        [Column(TypeName = "varchar(40)")]
        public string? PHIAreaId { get; set; }
        public virtual PHIArea? PHIArea { get; set; }
    }
}