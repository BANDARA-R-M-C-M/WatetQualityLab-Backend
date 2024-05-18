using System.ComponentModel.DataAnnotations;

namespace Project_v1.Models
{
    public class Lab {
        [Key]
        public string LabID { get; set; }
        public string LabName { get; set; }
        public string LabLocation { get; set; }
        public string LabTelephone { get; set; }
        public virtual ICollection<SystemUser> Mlts { get; set; } = new List<SystemUser>();
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
        public virtual ICollection<MOHArea> MOHAreas { get; set; } = new List<MOHArea>();
        public virtual ICollection<GeneralCategory> GeneralCategories { get; set; } = new List<GeneralCategory>();
        public virtual ICollection<InstrumentalQualityControl> InstrumentalQualityControls { get; set;} = new List<InstrumentalQualityControl>();
        public virtual ICollection<MediaQualityControl> MediaQualityControls { get; set; }= new List<MediaQualityControl>();
    }
}