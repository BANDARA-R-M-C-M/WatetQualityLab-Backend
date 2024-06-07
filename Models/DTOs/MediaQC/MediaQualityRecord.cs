using System.ComponentModel.DataAnnotations;

namespace Project_v1.Models.DTOs.MediaQC {
    public class MediaQualityRecord {
        [Required]
        [StringLength(40, ErrorMessage = "Content not exceed 40 characters")]
        [DataType(DataType.Text)]
        public string MediaId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateTime { get; set; }

        [StringLength(40, ErrorMessage = "Content not exceed 40 characters")]
        [DataType(DataType.Text)]
        public string? Sterility { get; set; }

        [StringLength(40, ErrorMessage = "Content not exceed 40 characters")]
        [DataType(DataType.Text)]
        public string? Stability { get; set; }

        [StringLength(40, ErrorMessage = "Content not exceed 40 characters")]
        [DataType(DataType.Text)]
        public string? Sensitivity { get; set; }

        [StringLength(150, ErrorMessage = "Content not exceed 150 characters")]
        [DataType(DataType.Text)]
        public string? Remarks { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Content not exceed 40 characters")]
        [DataType(DataType.Text)]
        public string MltId { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Content not exceed 40 characters")]
        [DataType(DataType.Text)]
        public string LabId { get; set; }
    }
}
