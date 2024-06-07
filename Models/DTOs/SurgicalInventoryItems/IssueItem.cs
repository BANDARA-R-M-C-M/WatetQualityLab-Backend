using System.ComponentModel.DataAnnotations;

namespace Project_v1.Models.DTOs.SurgicalInventoryItems {
    public class IssueItem {
        [Required]
        [StringLength(40, ErrorMessage = "Cannot enter more than 40 characters")]
        [DataType(DataType.Text)]
        public string ItemId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set;}

        [Required]
        [StringLength(40, ErrorMessage = "Cannot enter more than 40 characters")]
        [DataType(DataType.Text)]
        public string IssuedBy { get; set; }

        [StringLength(150, ErrorMessage = "Cannot enter more than 150 characters")]
        [DataType(DataType.Text)]
        public string Remarks { get; set; }
    }
}
