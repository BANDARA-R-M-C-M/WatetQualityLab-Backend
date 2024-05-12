using System.ComponentModel.DataAnnotations;
using Project_v1.Models.Users;

namespace Project_v1.Models {
    public class Lab {
        [Key]
        public string LabID { get; set; }
        public string LabName { get; set; }
        public string LabLocation { get; set; }
        public string LabTelephone { get; set; }
        /*public virtual GeneralInventory GeneralInventory { get; set; }*/
        public virtual ICollection<SystemUser> Mlts { get; set; } = new List<SystemUser>();
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
        public virtual ICollection<MOHArea> MOHAreas { get; set; } = new List<MOHArea>();
        public virtual ICollection<GeneralInventory> GeneralInventory { get; set; } = new List<GeneralInventory>();
    }
}