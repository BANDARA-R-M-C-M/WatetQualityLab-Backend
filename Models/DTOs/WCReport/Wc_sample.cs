using System.ComponentModel.DataAnnotations;

namespace Project_v1.Models.DTOs.WCReport {
    public class Wc_sample {
        [Required]
        [StringLength(20, ErrorMessage = "Content no exeed 20 characters")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[A-Z]{2}\([A-Z]{2}\)\/PHI\/\d{2}\/\d{4}$", ErrorMessage = "Invalid format")]
        public string YourRefNo { get; set; }

        [Required]
        [StringLength(25, ErrorMessage = "Content no exeed 25 characters")]
        [DataType(DataType.Text)]
        public string StateOfChlorination { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly DateOfCollection { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Content no exeed 40 characters")]
        [DataType(DataType.Text)]
        public string CatagoryOfSource { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Content no exeed 40 characters")]
        [DataType(DataType.Text)]
        public string CollectingSource { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Content no exeed 40 characters")]
        [DataType(DataType.Text)]
        public string phiId { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Content no exeed 40 characters")]
        [DataType(DataType.Text)]
        public string phiAreaName { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Content no exeed 40 characters")]
        [DataType(DataType.Text)]
        public string PHIAreaID { get; set; }
    }
}