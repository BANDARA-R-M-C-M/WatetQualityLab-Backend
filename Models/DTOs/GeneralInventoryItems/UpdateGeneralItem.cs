using System.ComponentModel.DataAnnotations;

namespace Project_v1.Models.DTOs.GeneralInventoryItems {
    public class UpdateGeneralItem {
        [Required]
        [StringLength(30, ErrorMessage = "Cannot enter more than 30 characters")]
        public string ItemName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly IssuedDate { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Cannot enter more than 40 characters")]
        [DataType(DataType.Text)]
        public string IssuedBy { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "Cannot enter more than 150 characters")]
        [DataType(DataType.Text)]
        public string Remarks { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Cannot enter more than 40 characters")]
        [DataType(DataType.Text)]
        public string GeneralCategoryID { get; set; }
    }
}
