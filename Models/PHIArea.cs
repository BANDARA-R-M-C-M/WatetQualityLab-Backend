using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_v1.Models
{
    public class PHIArea {
        [Key]
        public string PHIAreaID { get; set; }
        public string PHIAreaName { get; set; }
        public string MOHAreaId { get; set; }
        public virtual MOHArea MOHArea { get; set; }
        public virtual ICollection<SystemUser> Phis { get; set; } = new List<SystemUser>();
        public virtual ICollection<Sample> Samples { get; set; } = new List<Sample>();
    }
}