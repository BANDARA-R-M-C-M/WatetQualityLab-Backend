using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_v1.Models {
    public class GeneralInventory {
        [Key]
        [Column(TypeName = "varchar(40)")]
        public string GeneralInventoryID { get; set; }

        [Column(TypeName = "varchar(30)")]
        public string ItemName { get; set; }

        [Column(TypeName = "date")]
        public DateOnly IssuedDate { get; set; }

        [Column(TypeName = "varchar(40)")]
        public string IssuedBy { get; set;}

        [Column(TypeName = "varchar(150)")]
        public string Remarks { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string ItemQR { get; set; }

        [ForeignKey("GeneralCategory")]
        [Column(TypeName = "varchar(40)")]
        public string GeneralCategoryID { get; set; }
        public virtual GeneralCategory GeneralCategory { get; set; }
    }
}
