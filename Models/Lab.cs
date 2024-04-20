using System.ComponentModel.DataAnnotations;
using Project_v1.Models.Users;

namespace Project_v1.Models {
    public class Lab {
        [Key]
        public string LabID { get; set; }
        public string Lab_name { get; set; }
        public string Lab_location { get; set; }
        public string Lab_telephone { get; set; }
        /*public virtual ICollection<Mlt> Mlts { get; set; } = new List<Mlt>();*/
        public virtual ICollection<SystemUser> Mlts { get; set; } = new List<SystemUser>();
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
        public virtual ICollection<PHIArea> PHIAreas { get; set; } = new List<PHIArea>();
    }
}