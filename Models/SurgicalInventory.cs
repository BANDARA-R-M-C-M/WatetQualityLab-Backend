using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_v1.Models {
    public class SurgicalInventory {
        [Key]
        [Column(TypeName = "varchar(40)")]
        public string SurgicalInventoryID { get; set; }

        [Column(TypeName = "varchar(40)")]
        public string ItemName { get; set; }

        [Column(TypeName = "date")]
        public DateOnly IssuedDate { get; set; }

        [Column(TypeName = "varchar(40)")]
        public string IssuedBy { get; set; }

        [Column(TypeName = "int")]
        public int Quantity { get; set; }

        [Column(TypeName = "varchar(150)")]
        public string Remarks { get; set; }

        [ForeignKey("SurgicalCategory")]
        [Column(TypeName = "varchar(40)")]
        public string SurgicalCategoryID { get; set; }
        public virtual SurgicalCategory SurgicalCategory { get; set; }
        public virtual ICollection<IssuedItem> IssuedItems { get; set; } = new List<IssuedItem>();
    }
}
