using System.ComponentModel.DataAnnotations;

namespace Project_v1.Models.DTOs.SurgicalInventoryItems {
    public class UpdateSurgicalCategory {
        [Required]
        [StringLength(30, ErrorMessage = "Cannot enter more than 30 characters")]
        [DataType(DataType.Text)]
        public string SurgicalCategoryName { get; set; }
    }
}
