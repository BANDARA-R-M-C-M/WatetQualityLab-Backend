using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_v1.Models {
    public class GeneralCategory {
        [Key]
        [Column(TypeName = "varchar(40)")]
        public string GeneralCategoryID { get; set; }

        [Column(TypeName = "varchar(30)")]
        public string GeneralCategoryName { get; set; }

        [ForeignKey("Lab")]
        [Column(TypeName = "varchar(40)")]
        public string LabId { get; set; }
        public virtual Lab Lab { get; set; }
        public virtual ICollection<GeneralInventory> GeneralInventories { get; set; } = new List<GeneralInventory>();
    }
}
