using System.ComponentModel.DataAnnotations;

namespace Project_v1.Models.DTOs.GeneralInventoryItems {
    public class NewGeneralItem {
        [Required]
        [StringLength(40, ErrorMessage = "Cannot enter more than 40 characters")]
        [DataType(DataType.Text)]
        public string ItemName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly IssuedDate { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Cannot enter more than 40 characters")]
        public string IssuedBy { get; set; }

        [StringLength(150, ErrorMessage = "Cannot enter more than 150 characters")]
        public string Remarks { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Cannot enter more than 40 characters")]
        public string GeneralCategoryID { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Cannot enter more than 40 characters")]
        public string LabId { get; set; }
    }
}
