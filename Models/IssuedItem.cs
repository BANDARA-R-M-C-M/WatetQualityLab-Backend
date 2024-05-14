using System.ComponentModel.DataAnnotations;

namespace Project_v1.Models {
    public class IssuedItem {
        [Key]
        public string IssuedItemID { get; set; }
        public string IssuedQuantity { get; set; }
        public DateOnly IssuedDate { get; set; }
        public string IssuedBy { get; set; }
        public string Remarks { get; set; }
        public string SurgicalInventoryID { get; set; }
        public virtual SurgicalInventory SurgicalInventory { get; set; }
    }
}
