using System.ComponentModel.DataAnnotations;
using Project_v1.Models.Users;

namespace Project_v1.Models {
    public class PHIArea {
        [Key]
        public string PHIAreaID { get; set; }
        public string PHIArea_name { get; set; }
        public string MOHAreaId { get; set; }
        public virtual MOHArea MOHArea { get; set; }
        public virtual ICollection<Phi> Phis { get; set; } = new List<Phi>();
        public virtual ICollection<Sample> Samples { get; set; } = new List<Sample>();
    }
}