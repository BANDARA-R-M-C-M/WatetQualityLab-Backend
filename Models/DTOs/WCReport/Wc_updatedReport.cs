using System.ComponentModel.DataAnnotations;

namespace Project_v1.Models.DTOs.WCReport {
    public class Wc_updatedReport {
        [Required]
        [StringLength(20, ErrorMessage = "Content no exeed 20 characters")]
        [DataType(DataType.Text, ErrorMessage = "")]
        [RegularExpression(@"^\d{4}\/\d{2}\/\d{2}$", ErrorMessage = "Invalid format")]
        public string MyRefNo { get; set; }

        [Required]
        [StringLength(10)]
        [DataType(DataType.Text)]
        public string AppearanceOfSample { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Count must be a non-negative number")]
        public int PresumptiveColiformCount { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Count must be a non-negative number")]
        public int EcoliCount { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Cannot enter more tha 200 characters")]
        public string Remarks { get; set; }

        [Required]
        public bool Contaminated { get; set; }
    }
}
