using System.ComponentModel.DataAnnotations;

namespace Project_v1.Models.DTOs.WCReport {
    public class SampleStatus {
        [Required]
        [StringLength(40, ErrorMessage = "Content no exeed 40 characters")]
        [DataType(DataType.Text)]
        public string SampleId { get; set; }

        [Required]
        [StringLength(8, ErrorMessage = "Content no exeed 8 characters")]
        [DataType(DataType.Text)]
        public string Status { get; set; }

        [StringLength(70, ErrorMessage = "Cannot enter more than 70 characters")]
        [DataType(DataType.Text)]
        public string? Comment { get; set; }
    }
}
