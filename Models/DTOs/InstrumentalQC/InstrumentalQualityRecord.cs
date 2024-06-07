using System.ComponentModel.DataAnnotations;

namespace Project_v1.Models.DTOs.InstrumentalQC {
    public class InstrumentalQualityRecord {
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateTime { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Content not exceed 40 characters")]
        [DataType(DataType.Text)]
        public string InstrumentId { get; set; }

        [Range(double.MinValue, double.MaxValue, ErrorMessage = "Temperature Fluctuation must be a valid number")]
        public double? TemperatureFluctuation { get; set; }

        [Range(double.MinValue, double.MaxValue, ErrorMessage = "Temperature Fluctuation must be a valid number")]
        public double? PressureGradient { get; set; }

        [StringLength(40, ErrorMessage = "Content not exceed 40 characters")]
        [DataType(DataType.Text)]
        public string? Timer { get; set; }

        [StringLength(40, ErrorMessage = "Content not exceed 40 characters")]
        [DataType(DataType.Text)]
        public string? Sterility { get; set; }

        [StringLength(40, ErrorMessage = "Content not exceed 40 characters")]
        [DataType(DataType.Text)]
        public string? Stability { get; set; }

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
