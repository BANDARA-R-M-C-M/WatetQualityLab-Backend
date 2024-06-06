using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_v1.Models
{
    public class MOHArea {
        [Key]
        [Column(TypeName = "varchar(40)")]
        public string MOHAreaID { get; set; }

        [Column(TypeName = "varchar(30)")]
        public string MOHAreaName { get; set; }

        [ForeignKey("Lab")]
        [Column(TypeName = "varchar(40)")]
        public string LabID { get; set; }
        public virtual Lab Lab { get; set; }
        public virtual ICollection<PHIArea> PHIAreas { get; set; } = new List<PHIArea>();
        public virtual ICollection<SystemUser> Moh_supervisors { get; set; } = new List<SystemUser>();
    }
}