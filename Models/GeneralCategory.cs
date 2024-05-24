using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_v1.Models {
    public class GeneralCategory {
        [Key]
        public string GeneralCategoryID { get; set; }
        public string GeneralCategoryName { get; set; }
        public string LabId { get; set; }
        public virtual Lab Lab { get; set; }
        public virtual ICollection<GeneralInventory> GeneralInventories { get; set; } = new List<GeneralInventory>();
    }
}
