using System.ComponentModel.DataAnnotations;
using Project_v1.Models.Users;

namespace Project_v1.Models {
    public class Lab {
        [Key]
        public string LabID { get; set; }
        public string LabName { get; set; }
        public string LabLocation { get; set; }
        public string LabTelephone { get; set; }
        /*public virtual ICollection<Mlt> Mlts { get; set; } = new List<Mlt>();*/
        public virtual ICollection<SystemUser> Mlts { get; set; } = new List<SystemUser>();
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
        /*public virtual ICollection<PHIArea> PHIAreas { get; set; } = new List<PHIArea>();*/
        public virtual ICollection<MOHArea> MOHAreas { get; set; } = new List<MOHArea>();//PHIArea kiyana eka MOHArea kiyl wens kara migration eka dunne na
    }
}