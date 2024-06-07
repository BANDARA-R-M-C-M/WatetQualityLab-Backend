using System.ComponentModel.DataAnnotations;

namespace Project_v1.Models.DTOs.SurgicalInventoryItems {
    public class UpdateSurgicalItem {
        [Required]
        [StringLength(40, ErrorMessage = "Cannot enter more than 40 characters")]
        [DataType(DataType.Text)]
        public string ItemName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly IssuedDate { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Cannot enter more than 40 characters")]
        [DataType(DataType.Text)]
        public string IssuedBy { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "Cannot enter more than 150 characters")]
        public string Remarks { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Cannot enter more than 40 characters")]
        public string SurgicalCategoryID { get; set; }
    }
}
