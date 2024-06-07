using System.ComponentModel.DataAnnotations;

namespace Project_v1.Models.DTOs.Areas {
    public class Moh_area {
        [Required]
        [StringLength(30, ErrorMessage = "Cannot enter more than 30 characters")]
        [DataType(DataType.Text)]
        public string MOHAreaName { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Cannot enter more than 40 characters")]
        [DataType(DataType.Text)]
        public string LabId { get; set; }
    }
}