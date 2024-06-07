using System.ComponentModel.DataAnnotations;

namespace Project_v1.Models.DTOs.WCReport {
    public class Wc_updatedSample {
        [Required]
        [StringLength(20)]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[A-Z]{2}\([A-Z]{2}\)\/[A-Z]{3}\/\d{2}\/\d{4}$", ErrorMessage = "Invalid format")]
        public string YourRefNo { get; set; }

        [Required]
        [StringLength(10)]
        [DataType(DataType.Text)]
        public string StateOfChlorination { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly DateOfCollection { get; set; }

        [Required]
        [StringLength(40)]
        public string CatagoryOfSource { get; set; }

        [Required]
        [StringLength(40)]
        public string CollectingSource { get; set; }
    }
}
