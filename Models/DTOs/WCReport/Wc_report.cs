using System.ComponentModel.DataAnnotations;

namespace Project_v1.Models.DTOs.WCReport {
    public class Wc_report {
        [Required]
        [StringLength(20, ErrorMessage = "Content no exeed 20 characters")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^\d{4}\/\d{2}\/\d{2}$", ErrorMessage = "Invalid format")]
        public string MyRefNo { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Content no exeed 20 characters")]
        [DataType(DataType.Text)]
        public string PresumptiveColiformCount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly AnalyzedDate { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Content no exeed 20 characters")]
        [DataType(DataType.Text)]
        public string EcoliCount { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Content no exeed 40 characters")]
        [DataType(DataType.Text)]
        public string AppearanceOfSample { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Cannot enter more tha 200 characters")]
        [DataType(DataType.Text)]
        public string Remarks { get; set; }

        [Required]
        public bool Contaminated { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Content no exeed 40 characters")]
        [DataType(DataType.Text)]
        public string MltId { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Content no exeed 40 characters")]
        [DataType(DataType.Text)]
        public string SampleId { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Content no exeed 40 characters")]
        [DataType(DataType.Text)]
        public string LabId { get; set; }
    }
}