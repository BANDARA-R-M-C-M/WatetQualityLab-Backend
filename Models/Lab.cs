using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_v1.Models
{
    public class Lab {
        [Key]
        [Column(TypeName = "varchar(40)")]
        public string LabID { get; set; }

        [Column(TypeName = "varchar(40)")]
        public string LabName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string LabLocation { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string LabTelephone { get; set; }
        public virtual ICollection<SystemUser> Mlts { get; set; } = new List<SystemUser>();
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
        public virtual ICollection<MOHArea> MOHAreas { get; set; } = new List<MOHArea>();
        public virtual ICollection<GeneralCategory> GeneralCategories { get; set; } = new List<GeneralCategory>();
        public virtual ICollection<InstrumentalQualityControl> InstrumentalQualityControls { get; set;} = new List<InstrumentalQualityControl>();
        public virtual ICollection<MediaQualityControl> MediaQualityControls { get; set; }= new List<MediaQualityControl>();
    }
}