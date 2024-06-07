using System.ComponentModel.DataAnnotations;

namespace Project_v1.Models.DTOs.Areas {
    public class lab {
        [Required]
        [StringLength(40, ErrorMessage = "Cannot enter more than 40 characters")]
        [DataType(DataType.Text)]
        public string LabName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Cannot enter more than 50 characters")]
        [DataType(DataType.Text)]
        public string LabLocation { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number format")]
        public string LabTelephone { get; set; }
    }
}
