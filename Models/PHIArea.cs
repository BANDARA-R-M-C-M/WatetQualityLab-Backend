using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_v1.Models
{
    public class PHIArea {
        [Key]
        [Column(TypeName = "varchar(40)")]
        public string PHIAreaID { get; set; }

        [Column(TypeName = "varchar(30)")]
        public string PHIAreaName { get; set; }

        [ForeignKey("MOHArea")]
        [Column(TypeName = "varchar(40)")]
        public string MOHAreaId { get; set; }
        public virtual MOHArea MOHArea { get; set; }
        public virtual ICollection<SystemUser> Phis { get; set; } = new List<SystemUser>();
        public virtual ICollection<Sample> Samples { get; set; } = new List<Sample>();
    }
}