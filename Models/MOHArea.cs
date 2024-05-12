using System.ComponentModel.DataAnnotations;
using Project_v1.Models.Users;

namespace Project_v1.Models {
    public class MOHArea {
        [Key]
        public string MOHAreaID { get; set; }
        public string MOHAreaName { get; set; }
        public string LabID { get; set; }
        public virtual Lab Lab { get; set; }
        public virtual ICollection<PHIArea> PHIAreas { get; set; } = new List<PHIArea>();
        public virtual ICollection<SystemUser> Moh_supervisors { get; set; } = new List<SystemUser>();
    }
}