using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_v1.Models {
    public class SurgicalCategory {
        [Key]
        [Column(TypeName = "varchar(40)")]
        public string SurgicalCategoryID { get; set; }

        [Column(TypeName = "varchar(30)")]
        public string SurgicalCategoryName { get; set; }

        [ForeignKey("Lab")]
        [Column(TypeName = "varchar(40)")]
        public string LabId { get; set; }
        public virtual Lab Lab { get; set; }
        public virtual ICollection<SurgicalInventory> SurgicalInventories { get; set; } = new List<SurgicalInventory>();
    }
}
