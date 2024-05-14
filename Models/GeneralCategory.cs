using System.ComponentModel.DataAnnotations;

namespace Project_v1.Models {
    public class GeneralCategory {
        [Key]
        public string GeneralCategoryID { get; set; }
        public string CategoryName { get; set; }
        public string LabId { get; set; }
        public virtual Lab Lab { get; set; }
        public virtual ICollection<GeneralInventory> GeneralInventories { get; set; } = new List<GeneralInventory>();
    }
}
