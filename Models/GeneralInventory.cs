using Humanizer;
using System.ComponentModel.DataAnnotations;

namespace Project_v1.Models {
    public class GeneralInventory {
        [Key]
        public string GeneralInventoryID { get; set; }
        public string ItemName { get; set; }
        public DateOnly IssuedDate { get; set; }
        public string IssuedBy { get; set;}
        public string Remarks { get; set; }
        public string GeneralCategoryID { get; set; }
        public virtual GeneralCategory GeneralCategory { get; set; }
        /*public string LabId { get; set; }
        public virtual Lab Lab { get; set; }*/
    }
}
