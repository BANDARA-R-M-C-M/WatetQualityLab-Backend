using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_v1.Models {
    public class IssuedItem {
        [Key]
        [Column(TypeName = "varchar(40)")]
        public string IssuedItemID { get; set; }

        [Column(TypeName = "int")]
        public int IssuedQuantity { get; set; }

        [Column(TypeName = "date")]
        public DateOnly IssuedDate { get; set; }

        [Column(TypeName = "varchar(40)")]
        public string IssuedBy { get; set; }

        [Column(TypeName = "varchar(150)")]
        public string Remarks { get; set; }

        [ForeignKey("SurgicalInventory")]
        [Column(TypeName = "varchar(40)")]
        public string SurgicalInventoryID { get; set; }
        public virtual SurgicalInventory SurgicalInventory { get; set; }
    }
}
