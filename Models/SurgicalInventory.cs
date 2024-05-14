using System.ComponentModel.DataAnnotations;

namespace Project_v1.Models {
    public class SurgicalInventory {
        [Key]
        public string SurgicalInventoryID { get; set; }
        public string ItemName { get; set; }
        public DateOnly IssuedDate { get; set; }
        public string IssuedBy { get; set; }
        public string Quantity { get; set; }
        public string Remarks { get; set; }
        public string SurgicalCategoryID { get; set; }
        public virtual SurgicalCategory SurgicalCategory { get; set; }
        public virtual ICollection<IssuedItem> IssuedItems { get; set; } = new List<IssuedItem>();
    }
}
