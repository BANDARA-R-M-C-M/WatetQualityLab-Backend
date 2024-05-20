using System.ComponentModel.DataAnnotations;

namespace Project_v1.Models {
    public class SurgicalCategory {
        [Key]
        public string SurgicalCategoryID { get; set; }
        public string SurgicalCategoryName { get; set; }
        public string LabId { get; set; }
        public virtual Lab Lab { get; set; }
        public virtual ICollection<SurgicalInventory> SurgicalInventories { get; set; } = new List<SurgicalInventory>();
    }
}
